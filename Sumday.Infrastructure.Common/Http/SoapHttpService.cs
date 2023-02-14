using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Infrastructure.Common.Messaging;

namespace Sumday.Infrastructure.Common.Http
{
    public abstract class SoapHttpService
    {
        private readonly IRequestBodySerializer requestBodySerializer;

        private readonly IResponseDeserializer responseDeserializer;

        private string beforeException = string.Empty;

        private string payLoadType = string.Empty;

        protected SoapHttpService()
        {
            this.requestBodySerializer = new SoapRequestBodySerializer(this.ConstructSoapRequest);
            this.responseDeserializer = new SoapResponseSerializer(this.FaultExceptionHandler);
        }

        protected abstract string SoapAction { get; }

        protected abstract HttpClient HttpClient { get; }

        protected EmailAddress[] ErrorNotifications { get; set; }

        protected IEmailSender EmailSender { get; set; }

        protected TelemetryClient TelemetryClient { get; set; }

        public virtual async Task<TResponse> Send<TBody, TResponse>(TBody payload, CancellationToken cancellationToken)
        {
            if (this.HttpClient == null)
            {
                throw new InvalidOperationException("HttpClient cannot be null");
            }

            if (this.SoapAction == null)
            {
                throw new InvalidOperationException("SoapAction cannot be null");
            }

            var postBody = this.requestBodySerializer.SerializeBody(payload);

            var request = new HttpRequestMessage
            {
                Content = new StringContent(postBody, Encoding.UTF8, "text/xml"),
                Method = HttpMethod.Post,
            };

            request.Headers.Add("SOAPAction", this.SoapAction);
            request.Content.Headers.Add("RequestType", payload.GetType().Name);
            this.payLoadType = payload.GetType().Name;
            var requestResponse = default(TResponse);
            this.beforeException = EnhancedStackTrace.Current().ToString();
            var response = await this.HttpClient.SendAsync(request, cancellationToken);
            string responseContent;
            if (response.Content != null)
            {
                responseContent = await ReadContentAsString(response, cancellationToken);
                try
                {
                    requestResponse = await this.responseDeserializer.Deserialize<TResponse>(postBody, responseContent);
                }
                catch (SoapException ex) when (Notify(ex))
                {
                }
            }

            response?.Dispose();
            return requestResponse;

            bool Notify(SoapException ex)
            {
                var stackTrace = this.beforeException + ex.ToStringDemystified();
                this.NotifyError("Request Deserialization Error", ex.Message, stackTrace, postBody, responseContent, cancellationToken).ConfigureAwait(false);

                return false;
            }
        }

        public virtual Task Notify<TBody>(string errorCode, string errorMessage, TBody payload, string responseMessage, CancellationToken cancellationToken)
        {
            var postBody = this.requestBodySerializer.SerializeBody(payload);
            var stackTrace = this.beforeException + EnhancedStackTrace.Current().ToString();
            return this.NotifyError(errorCode, errorMessage, stackTrace, postBody, responseMessage, cancellationToken);
        }

        protected abstract string ConstructSoapRequest(string request);

        protected abstract Task FaultExceptionHandler(Fault faultResponse, string requestMessage, string responseMessage);

        private static async Task<string> ReadContentAsString(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            // Check whether response is compressed
            if (response.Content.Headers.ContentEncoding.Any(x => x == "gzip"))
            {
                using var s = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var decompressed = new GZipStream(s, CompressionMode.Decompress);
                using var rdr = new StreamReader(decompressed);
                return await rdr.ReadToEndAsync();
            }
            else if (response.Content is StreamContent)
            {
                using var s = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var rdr = new StreamReader(s);
                return await rdr.ReadToEndAsync();
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        private async Task NotifyError(string errorCode, string errorMessage, string stackTrace, string requestMessage, string responseMessage, CancellationToken cancellationToken)
        {
            if (this.ErrorNotifications != null && this.ErrorNotifications.Length != 0 && this.EmailSender != null)
            {
                var environmentName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "LocalDev";
                var subject = $"{environmentName} - {this.GetType().Name} Exception";
                var emailMessage = new[]
                {
                    $"ErrorCode: {errorCode}",
                    $"ErrorMessage: {Uri.EscapeDataString(errorMessage)}",
                    $"Request: {Uri.EscapeDataString(requestMessage)}",
                    $"Response: {Uri.EscapeDataString(responseMessage)}",
                    $"StackTrace: {stackTrace}",
                };
                var body = string.Join($"{Environment.NewLine}{Environment.NewLine}", emailMessage);
                var parameters = new SendEmailParameters(this.ErrorNotifications.FirstOrDefault(), subject)
                {
                    Text = body,
                    Ccs = this.ErrorNotifications.Skip(1).ToArray(),
                };

                if (environmentName != "LocalDev")
                {
                    if (this.TelemetryClient != null)
                    {
                        this.TelemetryClient.TrackEvent("Soap Request Details", new Dictionary<string, string> { { "request", this.payLoadType } });
                    }

                    await this.EmailSender.SendEmailAsync(parameters, cancellationToken);
                }
            }
        }
    }
}
