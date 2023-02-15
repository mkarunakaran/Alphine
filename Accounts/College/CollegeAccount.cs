using Sumday.BoundedContext.ShareHolder.Accounts.College.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using System;

namespace Sumday.BoundedContext.ShareHolder.Accounts.College
{
    public sealed class CollegeAccount : Account
    {
        public CollegeAccount(AccountId id, decimal goal, DateTime createdDate, DateTime? modifiedDate, DateTime? convertedDate, DateTime? asOfPerformanceDate)
           : base(id, goal, createdDate, modifiedDate)
        {
            this.ConvertedDate = convertedDate;
            this.AsOfPerformanceDate = asOfPerformanceDate;
        }

        public override AccountType Type
        {
            get
            {
                if (this.Owner.CollegeAccountType == CollegeAccountType.Utma && this.Beneficiary != null && this.Beneficiary.Identity.Address.State.IsUgma)
                {
                    return CollegeAccountType.Ugma.Value;
                }

                return this.Owner.CollegeAccountType.Value;
            }
        }

        public CollegeAccountOwner Owner { get; private set; }

        public CollegeBeneficiary Beneficiary { get; private set; }

        public DateTime? ConvertedDate { get; }

        public DateTime? AsOfPerformanceDate { get; }

        public PersonalIdentity SuccessorOwner { get; private set; }

        public override bool CheckIsInValidState()
        {
            return true;
        }

        public void SetAccountOwner(CollegeAccountOwner owner)
        {
            this.Owner = owner;
            this.CheckIsInValidState();
        }

        public void SetBeneficiary(CollegeBeneficiary beneficiary)
        {
            this.Beneficiary = beneficiary;
            this.CheckIsInValidState();
        }

        public void SetSuccessor(PersonalIdentity successor)
        {
            this.SuccessorOwner = successor;
            this.CheckIsInValidState();
        }
    }
}
