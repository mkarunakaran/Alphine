using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.College.ValueObjects
{
    public sealed class CollegeAccountType : Enumeration<CollegeAccountType, string>
    {
        public CollegeAccountType(string type, string value)
         : base(type, value)
        {
        }

        public static CollegeAccountType CollegeIndividual => new CollegeAccountType("701", nameof(CollegeIndividual));

        public static CollegeAccountType Ugma => new CollegeAccountType("702g", nameof(Ugma));

        public static CollegeAccountType Utma => new CollegeAccountType("702", nameof(Utma));

        public static CollegeAccountType CollegeEntity => new CollegeAccountType("705", nameof(CollegeEntity));

        public static CollegeAccountType Trust => new CollegeAccountType("703", nameof(Trust));

        public static CollegeAccountType Corporate => new CollegeAccountType("704", nameof(Corporate));

        public static CollegeAccountType Organization => new CollegeAccountType("705", nameof(Organization));

        public static CollegeAccountType Government => new CollegeAccountType("706", nameof(Government));

        public static implicit operator CollegeAccountType(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(CollegeAccountType accountType) => accountType.Value;
    }
}
