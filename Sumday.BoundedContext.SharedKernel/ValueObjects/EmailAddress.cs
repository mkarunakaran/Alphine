using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class EmailAddress : Masked
    {
        private static readonly Regex EmailRegex = new Regex(RegularExpressions.Email, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        public EmailAddress(string email, string name = null)
        {
            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            Match match = EmailRegex.Match(email);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(EmailAddress));
            }

            this.Email = email;
            this.Name = name;
            if (string.IsNullOrEmpty(name))
            {
                var splitEmail = this.Email.Split('@', System.StringSplitOptions.RemoveEmptyEntries);
                this.Name = splitEmail[0];
            }
        }

        public string Email { get; }

        public string Name { get; }

        public static implicit operator EmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return new EmailAddress(email);
        }

        public static implicit operator string(EmailAddress emailAddress)
        {
            return emailAddress?.Email;
        }

        public override string ToString()
        {
            return this.Email;
        }

        public override string ToMaskedString()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                return this.Email;
            }

            var splitEmail = this.Email.Split('@', System.StringSplitOptions.RemoveEmptyEntries);

            if (splitEmail.Length != 2)
            {
                return this.Email;
            }

            var builder = new StringBuilder();
            for (var i = 0; i < splitEmail[0].Length; i++)
            {
                if (i == 0 || i == splitEmail[0].Length - 1)
                {
                    builder.Append(splitEmail[0][i]);
                }
                else
                {
                    builder.Append(Mask);
                }
            }

            builder.Append('@');
            builder.Append(splitEmail[1]);

            return builder.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Email;
            yield return this.Name;
        }
    }
}
