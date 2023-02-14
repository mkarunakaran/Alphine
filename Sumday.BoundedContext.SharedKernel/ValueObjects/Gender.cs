using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class Gender : Enumeration<Gender, string>
    {
        public Gender(string type, string value)
           : base(type, value)
        {
        }

        public static Gender UnKnown => new Gender("N", "UnKnown");

        public static Gender Male => new Gender("M", "Male");

        public static Gender Female => new Gender("F", "Female");

        public static implicit operator Gender(string value)
        {
           var gender = FromValue(value) ?? FromName(value);
           return gender;
        }

        public static implicit operator string(Gender gender) => gender?.Value;
    }
}
