using Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects;
using Sumday.Domain.Abstractions.ExitPorts;
using System;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able
{
    public sealed class AbleAccount : Account
    {
        public AbleAccount(AccountId id, decimal goal, AbleAccountStatus ableAccountStatus,  DateTime createdDate, DateTime? modifiedDate, DateTime? convertedDate, DateTime? asOfPerformanceDate)
            : base(id, goal, createdDate, modifiedDate)
        {
            this.ConvertedDate = convertedDate;
            this.AsOfPerformanceDate = asOfPerformanceDate;
            this.AbleAccountStatus = ableAccountStatus;
        }

        public override AccountType Type
        {
            get
            {
                return this.Owner.AbleAccountType.Value;
            }
        }

        public AbleAccountOwner Owner { get; private set; }

        public AbleBeneficiary Beneficiary { get; private set; }

        public DateTime? ConvertedDate { get; }

        public DateTime? AsOfPerformanceDate { get; }

        public AbleAccountStatus AbleAccountStatus { get; }

        public IExecuteErrorNotifcations SetAccountOwner(AbleAccountOwner owner)
        {
            this.Owner = owner;
            return this.CheckIsInValidState();
        }

        public IExecuteErrorNotifcations SetBeneficiary(AbleBeneficiary ableBeneficiary)
        {
            this.Beneficiary = ableBeneficiary;
            return this.CheckIsInValidState();
        }

        public override IExecuteErrorNotifcations CheckIsInValidState()
        {
            return new ExecutionResult();
        }
    }
}
