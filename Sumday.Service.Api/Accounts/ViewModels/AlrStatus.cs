using System;
using System.Collections.Generic;
using System.Text;

namespace Sumday
{
    public enum AlrStatus
    {
        /// <summary>
        //// This status is used when the user is prompted to upload the ALR verification form. It's the first status
        /// </summary>
        VerificationHold,

        /// <summary>
        /// This status is used when the user has uploaded their document and the ops team has not processed the memo.
        /// </summary>
        VerificationPending
    }
}
