using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public sealed class AbleAccountStatus : Enumeration<AbleAccountStatus, string>
    {
        private const string Pending = "PENDING";

        public AbleAccountStatus(string type, string value)
        : base(type, value)
        {
        }

        public static AbleAccountStatus VerificationHold => new AbleAccountStatus(nameof(VerificationHold), nameof(VerificationHold));

        public static AbleAccountStatus VerificationPending => new AbleAccountStatus(nameof(VerificationPending), nameof(Pending));

        public static AbleAccountStatus Approved => new AbleAccountStatus(nameof(Approved), string.Empty);

        public static AbleAccountStatus None => new AbleAccountStatus(nameof(None), nameof(Pending));

        public static implicit operator AbleAccountStatus(string value)
        {
            var status = FromValue(value);

            if (status == null)
            {
                status = VerificationHold;
                status.Value = value;
            }

            return status;
        }

        public static implicit operator string(AbleAccountStatus accountStatus) => accountStatus.Value;
    }
}
