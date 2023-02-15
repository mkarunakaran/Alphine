using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class AbleEntityOwnerType : Enumeration<AbleEntityOwnerType, string>
    {
        public AbleEntityOwnerType(string type, string value)
        : base(type, value)
        {
        }

        public static AbleEntityOwnerType AbleEntity => new AbleEntityOwnerType(AbleAccountType.AlrEntity.ToString(), nameof(AbleEntity));

        public static implicit operator AbleEntityOwnerType(string value) => FromValue(value);

        public static implicit operator string(AbleEntityOwnerType ableEntityOwnerType) => ableEntityOwnerType.Value;

        public static implicit operator AbleEntityOwnerType(AbleAccountType type) => FromValue(type.Value);
    }
}
