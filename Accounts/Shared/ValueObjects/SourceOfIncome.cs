using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public sealed class SourceOfIncome : Enumeration<SourceOfIncome, string>
    {
        public SourceOfIncome(string type, string value)
            : base(type, value)
        {
        }

        public static SourceOfIncome Other => new SourceOfIncome("OTH", "Other");

        public static SourceOfIncome GovernmentBenefits => new SourceOfIncome("GOV", "GovernmentBenefits");

        public static SourceOfIncome RetirementSavings => new SourceOfIncome("RET", "RetirementSavings");

        public static SourceOfIncome SocialSecurityOrPension => new SourceOfIncome("SOC", "SocialSecurityOrPension");

        public static SourceOfIncome SpousalSupport => new SourceOfIncome("SPS", "SpousalSupport");

        public static implicit operator SourceOfIncome(string value) => FromName(value) ?? FromValue(value);

        public static implicit operator string(SourceOfIncome employmentStatus) => employmentStatus.Value;
    }
}
