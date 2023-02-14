using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public enum CipStatus
    {
        /// <summary>
        /// Uknown status for CIP. Most likely the CIP process has not run yet.
        /// </summary>
        Unknown,

        /// <summary>
        /// The CIP process has run for the user and has passed or has been rectified.
        /// </summary>
        Passed,

        /// <summary>
        /// After running the CIP process there were errors.
        /// </summary>
        Failed
    }
}
