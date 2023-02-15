using Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.College.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public class AccountType : Enumeration<AccountType, string>
    {
        public AccountType(string value, string name)
       : base(value, name)
        {
        }

        public static AccountType Individual => new AccountType(nameof(Individual), nameof(Individual));

        public static AccountType Entity => new AccountType(nameof(Entity), nameof(Entity));

        public static AccountType RothIRA => new AccountType(nameof(RothIRA), nameof(RothIRA));

        public static AccountType TraditionalIRA => new AccountType(nameof(TraditionalIRA), nameof(TraditionalIRA));

        public static AccountType Trust => new AccountType(nameof(Trust), nameof(Trust));

        public static AccountType Ugma => new AccountType(nameof(Ugma), nameof(Ugma));

        public static AccountType Utma => new AccountType(nameof(Utma), nameof(Utma));

        public static AccountType Corporate => new AccountType(nameof(Corporate), nameof(Corporate));

        public static AccountType Government => new AccountType(nameof(Government), nameof(Government));

        public static AccountType Organization => new AccountType(nameof(Organization), nameof(Organization));

        public static implicit operator AccountType(string value)
        {
            AbleAccountType ableAccountType = value;

            if (ableAccountType != null)
            {
                AbleAccountOwnerType ableAccountOwnerType = ableAccountType;
                if (ableAccountOwnerType == AbleAccountOwnerType.AbleIndividual)
                {
                    return Individual;
                }

                return Entity;
            }

            CollegeAccountType collegeAccountType = value;

            if (collegeAccountType != null)
            {
                return FromValue(collegeAccountType.Value);
            }

            IRAAccountType iRAAccountType = value;
            if (iRAAccountType != null)
            {
                return FromValue(iRAAccountType.Value);
            }

            return FromValue(value) ?? FromName(value);
        }

        public static implicit operator string(AccountType accountType) => accountType.Value;
    }
}
