using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.ShareHolderInfo.ViewModels
{
    public class Employment
    {
        public EmploymentStatus Status { get; set; }

        public string Occupation { get; set; }

        public SourceOfIncome[] SourcesOfIncome { get; set; }

        public string SourcesOfIncomeOther { get; set; }
    }
}
