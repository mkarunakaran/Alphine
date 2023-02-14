using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class Ssn : Tin
    {
        public Ssn(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if ("0123456789".Contains(text.Replace("-", string.Empty)) || "9876543210".Contains(text.Replace("-", string.Empty)))
            {
                throw new InvalidObjectException(nameof(Ssn));
            }

            Regex regex = new Regex(RegularExpressions.Ssn);
            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(Ssn));
            }

            this.Text = text;
        }

        public string LastFour
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return string.Empty;
                }

                if (this.Text.Length > 4)
                {
                    return this.Text[^4..];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public override TinType TinType => TinType.SSN;

        public static implicit operator Ssn(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new Ssn(text);
        }

        public static implicit operator string(Ssn ssn)
        {
            return ssn?.Text;
        }

        public static string Format(string ssn)
        {
            var regex = new Regex(RegularExpressions.FormatSsn);
            var matches = regex.Match(ssn);

            if (matches?.Groups?.Count == 4)
            {
                return $"{matches.Groups[1].Value}-{matches.Groups[2].Value}-{matches.Groups[3].Value}";
            }

            return ssn;
        }

        public override string ToString()
        {
            return this.Text;
        }

        public override string ToMaskedString()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                return this.Text;
            }

            if (this.Text.Length < 9)
            {
                return this.Text;
            }

            return string.Format("{0}{0}{0}-{0}{0}-{1}", Mask, this.Text.Substring(this.Text.Length - 4, 4));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }
    }
}
