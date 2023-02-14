using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public sealed class TinType : Enumeration<TinType, string>
    {
        public TinType(string type, string value)
           : base(type, value)
        {
        }

        public static TinType SSN => new TinType(nameof(SSN), nameof(SSN));

        public static TinType EIN => new TinType(nameof(EIN), nameof(EIN));

        public static TinType CSIN => new TinType(nameof(SSN), nameof(CSIN)); // Convert CSIN to SSM

        public static implicit operator TinType(string value) => FromValue(value);

        public static implicit operator string(TinType tinType) => tinType.Value;
    }
}
