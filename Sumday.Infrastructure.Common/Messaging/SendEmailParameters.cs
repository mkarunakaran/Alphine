using System;
using Sumday.BoundedContext.SharedKernel.ValueObjects;

namespace Sumday.Infrastructure.Common.Messaging
{
    public class SendEmailParameters
    {
        private readonly string id;

        public SendEmailParameters(EmailAddress to, string subject)
        {
            this.To = to;
            this.Subject = subject;
            this.id = Guid.NewGuid().ToString();
        }

        public string CategoryId => $"{this.EmailType}-{this.id}";

        public EmailAddress To { get; set; }

        public string Subject { get; set; }

        public string Html { get; set; }

        public string Text { get; set; }

        public EmailAddress ReplyTo { get; set; }

        public EmailAddress[] Ccs { get; set; }

        public string EmailType { get; set; }

        public string Response { get; set; }

        public string MessageId { get; set; }

        public DateTime SendDate { get; set; }
    }
}
