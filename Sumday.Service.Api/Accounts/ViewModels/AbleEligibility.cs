using System;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public class AbleEligibility
    {
        public DateTime? IneligibleDate { get; set; }

        public int DiagnosisCode { get; set; }

        public bool PermanentDisability { get; set; }

        public AbleEligibilityReason? Reason { get; set; }
    }
}
