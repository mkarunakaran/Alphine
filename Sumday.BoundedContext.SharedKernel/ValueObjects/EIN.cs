using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class Ein : Tin
    {
        public Ein(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ShouldNotBeEmptyException(nameof(Ein));
            }

            Regex regex = new Regex(RegularExpressions.Ein);
            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(Ein));
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

        public override TinType TinType => TinType.EIN;

        public static implicit operator Ein(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new Ein(text);
        }

        public static implicit operator string(Ein ein)
        {
            return ein?.Text;
        }

        public static string Format(string ein)
        {
            var regex = new Regex(RegularExpressions.Ein);
            var matches = regex.Match(ein);

            if (matches?.Groups?.Count == 3)
            {
                return $"{matches.Groups[1].Value}-{matches.Groups[2].Value}";
            }

            return ein;
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

            return string.Format("{0}{0}-{0}{0}{0}{1}", Mask, this.Text.Substring(this.Text.Length - 4, 4));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }
    }
}
