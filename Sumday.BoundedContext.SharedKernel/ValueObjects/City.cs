using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class City : ValueObject
    {
        private const int MinLength = 1;
        private const int Maxlength = 22;

        public City(string text)
        {
            Regex regex = new Regex(RegularExpressions.AllowedCharacters);
            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(City));
            }

            if (text.Length < MinLength || text.Length > Maxlength)
            {
                throw new InvalidObjectException(nameof(AddressLine), $"This must be between {MinLength} and {Maxlength} characters in length.");
            }

            this.Text = text;
        }

        public string Text { get; }

        public static implicit operator City(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new City(text);
        }

        public static implicit operator string(City city)
        {
            return city?.Text;
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
