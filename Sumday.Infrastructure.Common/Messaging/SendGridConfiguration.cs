using Sumday.BoundedContext.SharedKernel.ValueObjects;

namespace Sumday.Infrastructure.Common.Messaging
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; }

        public EmailAddress From { get; set; }

        public string OverrideToEmail { get; set; }

        public string UnsubscribeMarketingGroup { get; set; }
    }
}