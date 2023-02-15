using Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using System;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public abstract class Account : Entity
    {
        protected Account(AccountId id, decimal goal,  DateTime createdDate, DateTime? modifiedDate)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
            this.ModifiedDate = modifiedDate;
            this.Goal = goal;
        }

        [EntityKey]
        public AccountId Id { get; }

        public abstract AccountType Type { get; }

        public DateTime CreatedDate { get; }

        public DateTime? ModifiedDate { get; }

        public decimal Goal { get; private set; }

        public abstract IExecuteErrorNotifcations CheckIsInValidState();
     }
}
