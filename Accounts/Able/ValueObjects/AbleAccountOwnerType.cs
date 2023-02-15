using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class AbleAccountOwnerType : Enumeration<AbleAccountOwnerType, string>
    {
        public AbleAccountOwnerType(string type, string value)
        : base(type, value)
        {
        }

        public static AbleAccountOwnerType AbleIndividual => new AbleAccountOwnerType(nameof(AbleIndividualOwnerType), nameof(AbleIndividual));

        public static AbleAccountOwnerType AbleEntity => new AbleAccountOwnerType(nameof(AbleEntityOwnerType), nameof(AbleEntity));

        public static implicit operator AbleAccountOwnerType(string value)
        {
            AbleIndividualOwnerType ableIndividualAccountOwnerType = value.ToString();
            if (ableIndividualAccountOwnerType != null)
            {
                return AbleIndividual;
            }
            else
            {
                AbleEntityOwnerType ableEntityOwnerType = value.ToString();
                if (ableEntityOwnerType != null)
                {
                    return AbleEntity;
                }
            }

            return null;
        }

        public static implicit operator string(AbleAccountOwnerType ableAccountOwnerType) => ableAccountOwnerType.Value;

        public static implicit operator AbleAccountOwnerType(AbleAccountType type) => type.Value;
    }
}
