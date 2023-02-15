using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public sealed class AbleAccountType : Enumeration<AbleAccountType, string>
    {
        public AbleAccountType(string value, string name)
         : base(value, name)
        {
        }

        public static AbleAccountType AMAO => new AbleAccountType("595", nameof(AMAO));

        public static AbleAccountType AlrAdult => new AbleAccountType("596", nameof(AlrAdult));

        public static AbleAccountType ALrMinor => new AbleAccountType("597", nameof(ALrMinor));

        public static AbleAccountType AlrEntity => new AbleAccountType("598", nameof(AlrEntity));

        public static implicit operator AbleAccountType(string type) => FromName(type) ?? FromValue(type);

        public static implicit operator string(AbleAccountType ableAccountType) => ableAccountType.Value;
    }
}
