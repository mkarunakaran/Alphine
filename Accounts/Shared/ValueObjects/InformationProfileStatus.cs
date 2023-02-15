using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class InformationProfileStatus : Enumeration<InformationProfileStatus, int>
    {
        public InformationProfileStatus(int type, string value)
         : base(type, value)
        {
        }

       // Uknown status for CIP.Most likely the CIP process has not run yet.
        public static InformationProfileStatus Unknown => new InformationProfileStatus(0, "Unknown");

        // The CIP process has run for the user and has passed or has been rectified.
        public static InformationProfileStatus Passed => new InformationProfileStatus(1, "Passed");

        // After running the CIP process there were errors.
        public static InformationProfileStatus Failed => new InformationProfileStatus(2, "Failed");

        public static implicit operator InformationProfileStatus(string value) => FromName(value);

        public static implicit operator string(InformationProfileStatus shareHolderIdentificationStatus) => shareHolderIdentificationStatus.Name;
    }
}
