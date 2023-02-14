using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    /// <summary>
    /// Customer Identification Program
    /// </summary>
    public class Cip
    {
        /// <summary>
        /// Gets or sets the CIP Status
        /// </summary>
        public CipStatus Status { get; set; }

        /// <summary>
        /// gets or sets the actual code value of the CIP Flag.
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the date that CIP was processed.
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Address
        /// </summary>
        public bool AddressErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Name
        /// </summary>
        public bool NameErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Date of Birth
        /// </summary>
        public bool DateOfBirthErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Identity
        /// </summary>
        public bool IdentityErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Ssn
        /// </summary>
        public bool SsnErrors { get; set; }
    }
}
