using Sumday.Domain.Abstractions;
using System.Linq;

namespace Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects
{
    public sealed class AccountStatus : Enumeration<AccountStatus, string>
    {
        public const string RolloverRequested = "Requested";

        public const string RolloverSubmitted = "Submitted";

        public const string RolloverPending = "Pending";

        public const string RolloverPendingDisbursement = "PendingDisbursement";

        public const string RolloverDisbursed = "Disbursed";

        public const string RolloverCompleted = "Completed";

        public const string RolloverFailed = "Failed";

        public AccountStatus(string type, string value)
        : base(type, value)
        {
        }

        public static AccountStatus Open => new AccountStatus(nameof(Open), nameof(Open));

        public static AccountStatus Closed => new AccountStatus(nameof(Closed), nameof(Closed));

        public static AccountStatus SetupCompleted => new AccountStatus(nameof(SetupCompleted), bool.TrueString);

        public static AccountStatus Rollover => new AccountStatus(nameof(Rollover), nameof(RolloverCompleted));

        public static implicit operator AccountStatus(string value)
        {
            var status = FromValue(value) ?? FromName(value);

            if (status == null)
            {
                status = Open;
                var rollOverValues = typeof(AccountStatus).GetConstantsValues<string>();
                if (rollOverValues.Contains(value))
                {
                    Rollover.Value = value;
                    status = Rollover;
                }
                else if (bool.TryParse(value, out bool result))
                {
                    SetupCompleted.Value = result.ToString();
                    status = SetupCompleted;
                }
            }

            return status;
        }

        public static implicit operator string(AccountStatus accountStatus) => accountStatus.Value;
    }
}
