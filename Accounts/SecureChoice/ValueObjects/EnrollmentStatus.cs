using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects
{
    public class EnrollmentStatus : Enumeration<EnrollmentStatus, string>
    {
        private const string Pending = "Account exists but the participant hasn't taken action yet within the 30-day opt-out period";

        private const string ActiveAutoEnrolled = "Participant's account has been 'activated' automatically by the end of the 30 days";

        private const string ActiveActivelyEnrolled = "Participant registered within the 30-day period";

        private const string ClosedPostEnrollmentOptout = "Participant had an active account and they choose to opt-out but keep account open / update the deferral rate to 0% / pause payroll";

        private const string ClosedPreEnrollmentOptout = "Participant had an active account and they choose to opt-out but keep account open / update the deferral rate to 0% / pause payroll";

        private const string Terminated = "Participant had an active account but they chose to opt out and close it";

        private const string NotEstablished = "Participant failed CIP, and  are still in CIP fail status for atleast 20 days";

        private EnrollmentStatus(string value, string name)
         : base(value, name)
        {
        }

        public static EnrollmentStatus PendingEnrollment => new EnrollmentStatus(nameof(Pending), nameof(Pending));

        public static EnrollmentStatus ActiveAutoEnrollment => new EnrollmentStatus("Active-auto-enrolled", nameof(ActiveAutoEnrolled));

        public static EnrollmentStatus ActiveActivelyEnrollment => new EnrollmentStatus("Active-actively-enrolled", nameof(ActiveActivelyEnrolled));

        public static EnrollmentStatus ClosedPostOptoutEnrollment => new EnrollmentStatus("Closed-post-enrollment-optout", nameof(ClosedPostEnrollmentOptout));

        public static EnrollmentStatus ClosedPreOptoutEnrollment => new EnrollmentStatus("Closed-pre-enrollment-optout", nameof(ClosedPreEnrollmentOptout));

        public static EnrollmentStatus ClosedOptoutEnrollment => new EnrollmentStatus("Terminated", nameof(Terminated));

        public static EnrollmentStatus ClosedCipFailed => new EnrollmentStatus("NotEstablished", nameof(ClosedCipFailed));

        public static implicit operator EnrollmentStatus(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(EnrollmentStatus enrollmentStatus) => enrollmentStatus.Value;
    }
}
