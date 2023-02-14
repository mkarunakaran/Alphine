using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class AddressLine : ValueObject
    {
        private const int MinLength = 1;
        private const int Maxlength = 42;
        private const string AddressPOBox = "That looks like a P.O. Box address. Try again with a home address.";

        public AddressLine(string text, bool allowPoBox = false)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ShouldNotBeEmptyException(nameof(AddressLine));
            }

            Regex regex = new Regex(RegularExpressions.AllowedCharacters);
            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(AddressLine));
            }

            if (text.Length < MinLength || text.Length > Maxlength)
            {
                throw new InvalidObjectException(nameof(AddressLine), $"This must be between {MinLength} and {Maxlength} characters in length.");
            }

            if (!allowPoBox)
            {
                Regex poRegex = new Regex(RegularExpressions.PoBox);
                if (poRegex.IsMatch(text))
                {
                    throw new InvalidObjectException(nameof(AddressLine), AddressPOBox);
                }
            }

            this.Text = text;
            this.AllowPoBox = allowPoBox;
        }

        public string Text { get; }

        public bool AllowPoBox { get; }

        public static implicit operator AddressLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new AddressLine(text);
        }

        public static implicit operator string(AddressLine line)
        {
            return line?.Text;
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
