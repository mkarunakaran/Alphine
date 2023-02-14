using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sumday.BoundedContext.SharedKernel.Exceptions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class Phone : Masked
    {
        public Phone(string text, PhoneType phoneType = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Regex regex = new Regex(RegularExpressions.Phone);
            Match match = regex.Match(text);

            if (!match.Success)
            {
                throw new InvalidObjectException(nameof(Phone));
            }

            this.Text = text;
            this.PhoneType = phoneType ?? PhoneType.Primary;
        }

        public string Text { get; }

        public PhoneType PhoneType { get; }

        public static implicit operator Phone(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new Phone(text);
        }

        public static implicit operator string(Phone phone)
        {
            return phone?.Text;
        }

        public static string Format(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length != 10)
            {
                return null;
            }

            return string.Concat(phone.Substring(0, 3), "-", phone.Substring(3, 3), "-", phone.Substring(6, 4));
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

            var phone = this.Text.Replace("-", string.Empty);

            return string.Format("{0}{0}{0}-{0}{0}{0}-{1}", Mask, phone[6..]);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }
    }
}
