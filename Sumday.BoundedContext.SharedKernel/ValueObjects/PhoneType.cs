using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class PhoneType : Enumeration<PhoneType, int>
    {
        public PhoneType(int type, string value)
         : base(type, value)
        {
        }

        public static PhoneType Primary => new PhoneType(0, "Primary");

        public static PhoneType Alternate => new PhoneType(1, "Alternate");

        public static implicit operator PhoneType(int value) => FromValue(value);

        public static implicit operator string(PhoneType phoneType) => phoneType.Name;
    }
}
