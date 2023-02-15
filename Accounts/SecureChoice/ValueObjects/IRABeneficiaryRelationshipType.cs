using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public sealed class IRABeneficiaryRelationshipType : Enumeration<IRABeneficiaryRelationshipType, string>
    {
        private const string CHILD = "CHILD";

        private const string SPOUSE = "SPOUSE";

        private const string OTHER = "OTHER";

        private const string RELATIVE = "RELATIVE";

        private IRABeneficiaryRelationshipType(string value, string name)
         : base(value, name)
        {
        }

        public static IRABeneficiaryRelationshipType Spouse => new IRABeneficiaryRelationshipType(nameof(SPOUSE), nameof(Spouse));

        public static IRABeneficiaryRelationshipType Child => new IRABeneficiaryRelationshipType(nameof(CHILD), nameof(Child));

        public static IRABeneficiaryRelationshipType Relative => new IRABeneficiaryRelationshipType(nameof(RELATIVE), nameof(Relative));

        public static IRABeneficiaryRelationshipType Other => new IRABeneficiaryRelationshipType(nameof(OTHER), nameof(Other));

        public static implicit operator IRABeneficiaryRelationshipType(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(IRABeneficiaryRelationshipType iRABeneficiaryType) => iRABeneficiaryType.Value;
    }
}
