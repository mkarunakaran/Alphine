using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class Name : ValueObject
    {
        private const int MinLength = 1;
        private const int Maxlength = 42;

        public Name(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ShouldNotBeEmptyException(nameof(Name));
            }

            Regex multipleSpacesRegex = new Regex(RegularExpressions.MultipleSpaces);
            text = multipleSpacesRegex.Replace(text ?? string.Empty, " ");

            Regex alphaDashAndSpaceAposRegex = new Regex(RegularExpressions.AlphaDashAndSpaceApos);
            Match match = alphaDashAndSpaceAposRegex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(Name));
            }

            Regex allowedCharactersRegex = new Regex(RegularExpressions.AllowedCharacters);
            match = allowedCharactersRegex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(Name));
            }

            if (text.Length < MinLength || text.Length > Maxlength)
            {
                throw new InvalidObjectException(nameof(Name), $"This must be between {MinLength} and {Maxlength} characters in length.");
            }

            this.Text = text;
        }

        public string Text { get; }

        public int Length => this.Text.Length;

        public static implicit operator Name(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new Name(text);
        }

        public static implicit operator string(Name name)
        {
            return name?.Text;
        }

        public override string ToString()
        {
            return this.Text.ToString();
        }

        public int IndexOf(char v)
        {
            return this.Text.IndexOf(v);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }
    }
}
