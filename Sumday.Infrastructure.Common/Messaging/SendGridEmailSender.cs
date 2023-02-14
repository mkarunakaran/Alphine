using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.Messaging
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IOptions<SendGridConfiguration> sendGridOptions;
        private readonly ILogger<SendGridConfiguration> logger;

        public SendGridEmailSender(IOptions<SendGridConfiguration> sendGridOptions, ILogger<SendGridConfiguration> logger)
        {
            this.sendGridOptions = sendGridOptions;
            this.logger = logger;
        }

        public async Task SendEmailAsync(SendEmailParameters parameters, CancellationToken cancellationToken)
        {
            var configuration = this.sendGridOptions.Value;
            var msg = new SendGridMessage()
            {
                From = new SendGrid.Helpers.Mail.EmailAddress(configuration.From.Email, configuration.From.Name),
                Subject = parameters.Subject,
                PlainTextContent = parameters.Text,
                HtmlContent = parameters.Html,
                Categories = new List<string> { parameters.CategoryId },
                CustomArgs = new Dictionary<string, string> { { "Id", parameters.CategoryId } }
            };

            if (parameters.ReplyTo != null && !string.IsNullOrEmpty(parameters.ReplyTo.Email))
            {
                msg.ReplyTo = new SendGrid.Helpers.Mail.EmailAddress(parameters.ReplyTo.Email, parameters.ReplyTo.Name);
                if (!string.IsNullOrEmpty(parameters.ReplyTo.Name))
                {
                    msg.From.Name = parameters.ReplyTo.Name;
                }
            }

            if (string.IsNullOrEmpty(configuration.OverrideToEmail))
            {
                msg.AddTo(new EmailAddress(parameters.To.Email, parameters.To.Name));
            }
            else
            {
                msg.AddTo(new SendGrid.Helpers.Mail.EmailAddress(configuration.OverrideToEmail));
            }

            if (parameters.Ccs?.Length > 0)
            {
                var emails = parameters.Ccs
                    .Where(address => !string.IsNullOrEmpty(address?.Email))
                    .Select(address => new SendGrid.Helpers.Mail.EmailAddress(address.Email, address.Name))
                    .ToList();

                msg.AddCcs(emails);
            }

            var client = new SendGridClient(configuration.ApiKey);

            try
            {
                var response = await client.SendEmailAsync(msg, cancellationToken);
                var body = await response.Body.ReadAsStringAsync(cancellationToken);
                if (response.Headers.TryGetValues("X-Message-ID", out var values))
                {
                    parameters.MessageId = string.Join(",", values);
                }

                if (response.Headers.TryGetValues("Date", out var date))
                {
                   if (DateTime.TryParse(date.FirstOrDefault() ?? DateTime.Today.ToString(), out var result))
                   {
                        parameters.SendDate = result.UtcToEasternStandardTime();
                   }
                }

                parameters.Response = $"Success-{response.StatusCode} -Response from SendGrid-{body}";
                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                {
                    parameters.Response = $"Status Code -{response.StatusCode} -Problem in sending email from sendGrid-{body}";
                    this.logger.LogError("Problem in sending email from sendGrid", body);
                }
            }
            catch (Exception ex)
            {
                  var errorMessage = new[]
                  {
                    $"ErrorMessage: Generic Exception-Problem in sending email from sendGrid-{ex.Message}",
                    $"ExceptionDetails: {ex.ToStringDemystified()}",
                  };
                  parameters.Response = string.Join($"{Environment.NewLine}{Environment.NewLine}", errorMessage);
            }
        }
    }
}
