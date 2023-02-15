using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public sealed class IRABeneficiaryType : Enumeration<IRABeneficiaryType, string>
    {
        private IRABeneficiaryType(string type, string value)
         : base(type, value)
        {
        }

        public static IRABeneficiaryType Primary => new IRABeneficiaryType("Pri", nameof(Primary));

        public static IRABeneficiaryType Contingent => new IRABeneficiaryType("Sec", nameof(Contingent));

        public static implicit operator IRABeneficiaryType(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(IRABeneficiaryType iRABeneficiaryType) => iRABeneficiaryType.Value;
    }
}
