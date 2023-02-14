using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sumday.Infrastructure.Common.Http
{
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public HttpClientLoggingHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public bool ExtendedLogging { get; set; } = true;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.Append("Request: ");
            stringBuilder.AppendLine(request.ToString());

            if (request.Content != null)
            {
                var body = await request.Content.ReadAsStringAsync(cancellationToken);
                stringBuilder.AppendLine("Request Body: ");
                stringBuilder.AppendLine(body);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            stringBuilder.Append("Response: ");
            stringBuilder.AppendLine(response.ToString());
            if (response.Content != null)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                stringBuilder.AppendLine("Response Body: ");
                stringBuilder.AppendLine(body);
            }

            if (this.ExtendedLogging)
            {
                this.ShowLog(response, stringBuilder.ToString());
            }
            else
            {
#if DEBUG
                this.ShowLog(response, stringBuilder.ToString());
#endif
            }

            return response;
        }

        private void ShowLog(HttpResponseMessage response, string log)
        {
            var statusCode = (int)response.StatusCode;
            if (statusCode >= 400 && statusCode < 500)
            {
                this.logger.LogWarning(log);
            }
            else if (statusCode >= 500 && statusCode < 600)
            {
                this.logger.LogError(log);
            }
            else
            {
                this.logger.LogInformation(log);
            }
        }
    }
}
