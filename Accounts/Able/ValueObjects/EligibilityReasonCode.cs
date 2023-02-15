using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class EligibilityReasonCode : Enumeration<EligibilityReasonCode, string>
    {
        private const string A = "Social Security Disability Insurance";
        private const string B = " Supplemental Security Income";
        private const string C = " Physician Certification";

        public EligibilityReasonCode(string value, string name)
         : base(value, name)
        {
        }

        public static EligibilityReasonCode SSDI => new EligibilityReasonCode(nameof(A), nameof(SSDI));

        public static EligibilityReasonCode SSI => new EligibilityReasonCode(nameof(B), nameof(SSI));

        public static EligibilityReasonCode PhysicianCertification => new EligibilityReasonCode(nameof(C), nameof(PhysicianCertification));

        public static implicit operator EligibilityReasonCode(string type) => FromValue(type) ?? FromName(type);

        public static implicit operator string(EligibilityReasonCode eligibilityReason) => eligibilityReason.Value;
    }
}
