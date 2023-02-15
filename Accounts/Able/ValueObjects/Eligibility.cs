using Sumday.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class Eligibility : ValueObject
    {
        private static readonly DateTime NonPermanentEligibleDate = new DateTime(1900, 01, 02);

        private static readonly DateTime PermanentEligibleDate = DateTime.MaxValue.ToUniversalTime().Date;

        public Eligibility(DiagnosisCode diagnosisCode,  DateTime? certificationDate, EligibilityReasonCode reason)
        {
            this.DiagnosisCode = diagnosisCode;
            this.CertificationDate = certificationDate;
            this.Reason = reason;
        }

        public DateTime? CertificationDate { get; }

        public DiagnosisCode DiagnosisCode { get; }

        public bool PermanentDisability => this.IsPermanentDisability();

        public EligibilityReasonCode Reason { get; }

        public DateTime? GetIneligibleDate()
        {
            if (this.CertificationDate.HasValue)
            {
                var certDate = this.CertificationDate.Value;
                if (certDate.Year == NonPermanentEligibleDate.Year ||
                    certDate.Year == PermanentEligibleDate.Year)
                {
                    return null;
                }
            }

            return this.CertificationDate;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.DiagnosisCode;
            yield return this.PermanentDisability;
            yield return this.CertificationDate;
            yield return this.Reason;
        }

        private bool IsPermanentDisability()
        {
            if (this.CertificationDate.HasValue)
            {
                var certDate = this.CertificationDate.Value;
                return certDate.Year == PermanentEligibleDate.Year;
            }

            return false;
        }
    }
}
