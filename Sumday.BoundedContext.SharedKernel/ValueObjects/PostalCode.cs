using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class PostalCode : ValueObject
    {
        public PostalCode(string text)
        {
            Regex regex = new Regex(RegularExpressions.PostalCode);
            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(PostalCode));
            }

            this.Text = text;
        }

        public string Text { get; }

        public string PostalCodeFirst5 => this.Text?.Substring(0, 5);

        public string PostalCodePlus4 => this.Text.Contains("-") ? this.Text.Split('-').ElementAtOrDefault(1) : null;

        public static implicit operator PostalCode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new PostalCode(text);
        }

        public static implicit operator string(PostalCode postalCode)
        {
            return postalCode?.Text;
        }

        public static string Format(string postalCode)
        {
            if (string.IsNullOrEmpty(postalCode))
            {
                return postalCode;
            }

            var regex = new Regex(RegularExpressions.PostalCode);
            var matches = regex.Match(postalCode);

            if (matches.Groups.Count == 3)
            {
                var last4 = matches.Groups[2].Value;

                if (!string.IsNullOrEmpty(last4))
                {
                    return $"{matches.Groups[1].Value}-{last4}";
                }

                return matches.Groups[1].Value;
            }

            if (matches.Groups.Count == 2)
            {
                return matches.Groups[1].Value;
            }

            return postalCode;
        }

        public override string ToString()
        {
            return this.Text;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }
    }
}
