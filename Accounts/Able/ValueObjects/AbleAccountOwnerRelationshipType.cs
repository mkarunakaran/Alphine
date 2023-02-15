using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public sealed class AbleAccountOwnerRelationshipType : Enumeration<AbleAccountOwnerRelationshipType, string>
    {
        private const string GUARDIAN = "GUARDIAN";

        private const string POA = "POA";

        private const string SELF = "SELF";

        private const string CONSRVTR = "CONSRVTR";

        private const string OTHER = "OTHER";

        public AbleAccountOwnerRelationshipType(string value, string name)
         : base(value, name)
        {
        }

        public static AbleAccountOwnerRelationshipType Self => new AbleAccountOwnerRelationshipType(nameof(SELF), nameof(Self));

        public static AbleAccountOwnerRelationshipType Conservator => new AbleAccountOwnerRelationshipType(nameof(CONSRVTR), nameof(Conservator));

        public static AbleAccountOwnerRelationshipType LegalGuardian => new AbleAccountOwnerRelationshipType(nameof(GUARDIAN), nameof(LegalGuardian));

        public static AbleAccountOwnerRelationshipType PowerOfAttorney => new AbleAccountOwnerRelationshipType(nameof(POA), nameof(PowerOfAttorney));

        public static AbleAccountOwnerRelationshipType Other => new AbleAccountOwnerRelationshipType(nameof(OTHER), nameof(Other));

        public static implicit operator AbleAccountOwnerRelationshipType(string valueOrName) => FromValue(valueOrName) ?? FromName(valueOrName);

        public static implicit operator string(AbleAccountOwnerRelationshipType ableOwnerRelationshipType) => ableOwnerRelationshipType.Name;
    }
}
