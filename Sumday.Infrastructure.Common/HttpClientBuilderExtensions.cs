using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Sumday.Infrastructure.Common
{
    public static class HttpClientBuilderExtensions
    {
        private static readonly RNGCryptoServiceProvider RngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public static void AddPolly(this IHttpClientBuilder builder)
        {
            builder.AddPolicyHandler((sp, request) => GetBulkHeadPolicy(sp, request))
                   .AddPolicyHandler((sp, request) => GetRetryPolicy(sp, request))
                   .AddPolicyHandler((sp, request) => GetCircuitBreakerPolicy(sp, request));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceProvider sp, HttpRequestMessage request)
        {
            var jitterer = new Random();
            var loggerFactory = sp.GetService<ILoggerFactory>();

            var retryCount = 3;
            var serverErrors = new HttpStatusCode[]
            {
                HttpStatusCode.BadGateway,
                HttpStatusCode.GatewayTimeout,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.RequestTimeout
            };

            return Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .Or<SocketException>()
                .OrResult(response => serverErrors.Contains(response.StatusCode))
            .WaitAndRetryAsync(
            retryCount,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                            TimeSpan.FromMilliseconds(GetJitter()),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                var requestType = request.RequestUri.AbsoluteUri;
                var values = default(IEnumerable<string>);
                if (request.Content?.Headers.TryGetValues("RequestType", out values) ?? false)
                {
                    // Can now check if the value is true:
                    requestType = values.FirstOrDefault();
                }

                var logger = loggerFactory.CreateLogger(requestType + retryAttempt);
                logger.LogError(
                    "Delaying {request} for {delay}ms, then making retry {retry}.",
                    requestType,
                    timespan.TotalMilliseconds,
                    retryAttempt,
                    outcome.Exception?.Message,
                    outcome.Exception?.ToString());
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceProvider sp, HttpRequestMessage request)
        {
            var loggerFactory = sp.GetService<ILoggerFactory>();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(30),
                (result, retryAttempt) =>
                {
                    var requestType = request.RequestUri.AbsoluteUri;
                    var values = default(IEnumerable<string>);
                    if (request.Content?.Headers.TryGetValues("RequestType", out values) ?? false)
                    {
                        // Can now check if the value is true:
                        requestType = values.FirstOrDefault();
                    }

                    var logger = loggerFactory.CreateLogger(requestType + retryAttempt);
                    logger.LogError("circuit broken");
                },
                () =>
                {
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetBulkHeadPolicy(IServiceProvider sp, HttpRequestMessage request)
        {
            var loggerFactory = sp.GetService<ILoggerFactory>();
            return Policy
               .BulkheadAsync<HttpResponseMessage>(4, 4, onBulkheadRejectedAsync: (context) =>
               {
                   var requestType = request.RequestUri.AbsoluteUri;
                   var values = default(IEnumerable<string>);
                   if (request.Content?.Headers.TryGetValues("RequestType", out values) ?? false)
                   {
                       // Can now check if the value is true:
                       requestType = values.FirstOrDefault();
                   }

                   var logger = loggerFactory.CreateLogger(requestType + context.CorrelationId);
                   logger.LogInformation("Bulk head execcuted");
                   return Task.CompletedTask;
               });
        }

        private static int GetJitter()
        {
            var randomNumber = new byte[1];
            RngCryptoServiceProvider.GetBytes(randomNumber);
            return randomNumber[0] % 100;
        }
    }
}
