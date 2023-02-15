using Sumday.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class InformationProfile : ValueObject
    {
        private static readonly List<int> AddressErrorCodes = new List<int> { 1, 6, 7, 8, 9, 16, 17, 18, 19, 20, 21, 24, 25, 27 };

        private static readonly List<int> NameErrorCodes = new List<int> { 2, 6, 10, 11, 12, 16, 17, 18, 21, 22, 24, 25, 26, 27 };

        private static readonly List<int> DateOfBirthErrorCodes = new List<int> { 3, 7, 10, 13, 14, 16, 19, 20, 21, 22, 23, 24, 25, 26, 27 };

        private static readonly List<int> IdentityErrorCodes = new List<int> { 4, 8, 11, 13, 15, 17, 19, 21, 23, 24, 26, 27 };

        private static readonly List<int> SsnErrorCodes = new List<int> { 5, 9, 12, 14, 15, 18, 20, 22, 23, 25, 26, 27 };

        public InformationProfile(string statusCode, DateTime? processedDate = null)
        {
            this.StatusCode = statusCode;
            this.ProcessedDate = processedDate;
            this.SetStatusAndErrors(statusCode);
        }

        public InformationProfileStatus Status { get; private set; }

        /// <summary>
        /// gets the actual code value of the CIP Flag.
        /// </summary>
        public string StatusCode { get; }

        /// <summary>
        /// Gets  the date that CIP was processed.
        /// </summary>
        public DateTime? ProcessedDate { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Address.
        /// </summary>
        public bool AddressErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Name.
        /// </summary>
        public bool NameErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Date of Birth.
        /// </summary>
        public bool DateOfBirthErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Identity.
        /// </summary>
        public bool IdentityErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not there were errors in the Ssn.
        /// </summary>
        public bool SsnErrors { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Status;
            yield return this.StatusCode;
        }

        private void SetStatusAndErrors(string statusCode)
        {
            this.Status = InformationProfileStatus.Unknown;
            if (!string.IsNullOrEmpty(statusCode) && statusCode.Equals("P", StringComparison.OrdinalIgnoreCase))
            {
                this.Status = InformationProfileStatus.Passed;
                return;
            }

            if (int.TryParse(statusCode, out int code))
            {
                this.AddressErrors = AddressErrorCodes.Contains(code);
                this.NameErrors = NameErrorCodes.Contains(code);
                this.DateOfBirthErrors = DateOfBirthErrorCodes.Contains(code);
                this.IdentityErrors = IdentityErrorCodes.Contains(code);
                this.SsnErrors = SsnErrorCodes.Contains(code);

                if (this.AddressErrors || this.NameErrors || this.DateOfBirthErrors || this.IdentityErrors || this.SsnErrors)
                {
                    this.Status = InformationProfileStatus.Failed;
                }
            }
        }
    }
}
