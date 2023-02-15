using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class AbleIndividualOwnerType : Enumeration<AbleIndividualOwnerType, string>
    {
        public AbleIndividualOwnerType(string value, string name)
       : base(value, name)
        {
        }

        public static AbleIndividualOwnerType AbleSelf => new AbleIndividualOwnerType(AbleAccountType.AMAO.Value, nameof(AbleSelf));

        public static AbleIndividualOwnerType AbleAlrAdultBeneficiary => new AbleIndividualOwnerType(AbleAccountType.AlrAdult.Value, nameof(AbleAlrAdultBeneficiary));

        public static AbleIndividualOwnerType AbleAlrMinorBeneficiary => new AbleIndividualOwnerType(AbleAccountType.ALrMinor.Value, nameof(AbleAlrMinorBeneficiary));

        public static implicit operator AbleIndividualOwnerType(string value)
        {
            var individualType = FromValue(value) ?? FromName(value);
            return individualType;
        }

        public static implicit operator AbleIndividualOwnerType(AbleAccountType type) => FromValue(type.Value);

        public static implicit operator string(AbleIndividualOwnerType ableIndividualOwnerType) => ableIndividualOwnerType.Value;
    }
}
