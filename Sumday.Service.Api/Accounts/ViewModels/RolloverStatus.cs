using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public enum RolloverStatus
    {
        Requested,

        Submitted,

        Pending,

        PendingDisbursement,

        Disbursed,

        Completed,

        Failed,

        //// These are not used in the code, but are used on the WA GET side

        EftNotificationReceived,

        Cancelled,
    }
}
