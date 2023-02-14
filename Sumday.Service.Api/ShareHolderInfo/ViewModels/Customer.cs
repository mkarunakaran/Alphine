using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.ShareHolderInfo.ViewModels
{
    public class Customer
    {
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the key that is used by the imaging system when Accounts are queued in TurboApp.
        /// </summary>
        public string QueueSourceKey { get; set; }

        public string PrimaryAccountNumber { get; set; }

        public string Email { get; set; }

         public PaperStatementPreference PaperStatementPreference { get; set; }

        public Employment Employment { get; set; }

        public DateTime? LastLogonDate { get; set; }

        public bool IsQueued { get; set; }

        public string StatementAndTaxFormContent { get; set; }
    }
}
