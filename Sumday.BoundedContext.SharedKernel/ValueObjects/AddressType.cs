using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class AddressType : Enumeration<AddressType, int>
    {
        public AddressType(int type, string value)
          : base(type, value)
        {
        }

        public static AddressType Primary => new AddressType(0, "Primary");

        public static AddressType Mailing => new AddressType(1, "Mailing");

        public static implicit operator AddressType(int value) => FromValue(value);

        public static implicit operator int(AddressType addressType) => addressType.Value;
    }
}
