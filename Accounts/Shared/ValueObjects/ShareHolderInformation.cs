using Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects;
using Sumday.Domain.Abstractions;
using System.Collections.Generic;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public class ShareHolderInformation : ValueObject
    {
        public ShareHolderInformation(PersonalIdentity shareHolderIdentity, string primaryAccountNumber, AccountType primaryAccountType, PaperStatementPreference paperStatementPreference)
        {
            this.ShareHolderIdentity = shareHolderIdentity;
            this.PrimaryAccountNumber = primaryAccountNumber;
            this.PrimaryAccountType = primaryAccountType;
            this.PaperStatementPreference = paperStatementPreference;
        }

        public PersonalIdentity ShareHolderIdentity { get; }

        public string PrimaryAccountNumber { get; set; }

        public AccountType PrimaryAccountType { get; set; }

        public PaperStatementPreference PaperStatementPreference { get; set; }

        public void ChangeStatementPreference(PaperStatementPreference paperStatementPreference)
        {
            this.PaperStatementPreference = paperStatementPreference;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.ShareHolderIdentity;
            yield return this.PrimaryAccountNumber;
            yield return this.PaperStatementPreference;
        }
    }
}
