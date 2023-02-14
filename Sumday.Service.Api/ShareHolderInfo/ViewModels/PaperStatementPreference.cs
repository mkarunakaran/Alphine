using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.ShareHolderInfo.ViewModels
{
    public enum PaperStatementPreference
    {
        /// <summary>
        ///     Both Quarterly Statements and Tax Forms will not be sent via snail mail
        /// </summary>
        None,

        /// <summary>
        ///     Both Quarterly Statements and Tax Forms are sent via snail mail
        /// </summary>
        StatementAndTaxForm,

        /// <summary>
        ///     Only Tax Forms are sent via snail mail
        /// </summary>
        TaxForm
    }
}
