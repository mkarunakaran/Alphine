using System;
using Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice;
using Sumday.BoundedContext.ShareHolder.Accounts.SecureChoice.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public sealed class SecureChoiceAccount : Account
    {
        public SecureChoiceAccount(AccountId id, DateTime createdDate, DateTime? modifiedDate)
           : base(id, 0, createdDate, modifiedDate)
        {
        }

        public override AccountType Type
        {
            get
            {
                var owner = this.Owner as IRAAccountOwner;
                return owner.IRAAccountType.Value;
            }
        }

        public IRAAccountOwner Owner { get; private set; }

        public SecureChoiceBeneficiary Beneficiary { get; private set; }

        public void SetAccountOwner(IRAAccountOwner owner)
        {
            this.Owner = owner;
            this.CheckIsInValidState();
        }

        public void SetBeneficiary(SecureChoiceBeneficiary beneficiary)
        {
            this.Beneficiary = beneficiary;
            this.CheckIsInValidState();
        }

        public override bool CheckIsInValidState()
        {
            return true;
        }
    }
}
