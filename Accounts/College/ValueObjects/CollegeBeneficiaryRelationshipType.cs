using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.College.ValueObjects
{
    public sealed class CollegeBeneficiaryRelationshipType : Enumeration<CollegeBeneficiaryRelationshipType, string>
    {
        private const string CHILD = "CHILD";

        private const string SPOUSE = "SPOUSE";

        private const string GRCHILD = "GRCHILD";

        private const string RELATIVE = "RELATIVE";

        private const string SELF = "SELF";

        private const string OTHER = "OTHER";

        public CollegeBeneficiaryRelationshipType(string value, string name)
         : base(value, name)
        {
        }

        public static CollegeBeneficiaryRelationshipType Spouse => new CollegeBeneficiaryRelationshipType(nameof(SPOUSE), nameof(Spouse));

        public static CollegeBeneficiaryRelationshipType Child => new CollegeBeneficiaryRelationshipType(nameof(CHILD), nameof(Child));

        public static CollegeBeneficiaryRelationshipType Relative => new CollegeBeneficiaryRelationshipType(nameof(RELATIVE), nameof(Relative));

        public static CollegeBeneficiaryRelationshipType Other => new CollegeBeneficiaryRelationshipType(nameof(OTHER), nameof(Other));

        public static CollegeBeneficiaryRelationshipType GrandChild => new CollegeBeneficiaryRelationshipType(nameof(GRCHILD), nameof(GrandChild));

        public static CollegeBeneficiaryRelationshipType Self => new CollegeBeneficiaryRelationshipType(nameof(SELF), nameof(Self));

        public static implicit operator CollegeBeneficiaryRelationshipType(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(CollegeBeneficiaryRelationshipType collegeBeneficiaryRelationshipType) => collegeBeneficiaryRelationshipType.Value;
    }
}
