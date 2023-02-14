using System;
using System.Collections.Generic;
using Sumday.BoundedContext.SharedKernel.Exceptions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class BirthDate : Masked
    {
        private const int MaximumAge = 110;
        private const int MinimumAge = 18;

        public BirthDate(DateTime text)
        {
            CheckIsValid(text);
            this.Text = text.Date;
        }

        public DateTime Text { get; }

        public int Age => GetAge(this.Text);

        public int Year => this.Text.Year;

        public bool IsMinor => GetAge(this.Text) < MinimumAge;

        public static implicit operator BirthDate(DateTime dateOfBirth)
        {
            CheckIsValid(dateOfBirth);
            return new BirthDate(dateOfBirth);
        }

        public static implicit operator DateTime(BirthDate date)
        {
            return date.Text.Date;
        }

        public override string ToString()
        {
            return this.Text.ToShortDateString();
        }

        public override string ToMaskedString()
        {
            return string.Format("{0}{0}/{0}{0}/{0}{0}{0}{0}", Mask);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }

        private static void CheckIsValid(DateTime text)
        {
            if (text == DateTime.MinValue)
            {
                throw new InvalidObjectException(nameof(BirthDate));
            }

            if (text > DateTime.Now || GetAge(text) > MaximumAge)
            {
                throw new InvalidObjectException(nameof(BirthDate));
            }
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            var date = DateTime.Today;
            var age = date.Year - dateOfBirth.Year;
            if (dateOfBirth > date.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
