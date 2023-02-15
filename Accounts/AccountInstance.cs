using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.ValueObjects;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public class AccountInstance : AggregateRoot
    {
        public AccountInstance(AccountNumber accountNumber, Name name, PlanId planId, string repId, string branchId, decimal? totalBalance, AccountStatus status)
        {
            this.AccountNumber = accountNumber;
            this.PlanId = planId;
            this.Status = status;
            this.RepId = repId;
            this.BranchId = branchId;
            this.Name = name;
            this.TotalBalance = totalBalance;
        }

        [EntityKey]
        public AccountNumber AccountNumber { get; }

        public PlanId PlanId { get; }

        public Name Name { get; }

        public string RepId { get; }

        public string BranchId { get; }

        public AccountPerformance Performance { get; private set; }

        public decimal? TotalBalance { get; }

        public AccountStatus Status { get; private set; }

        public Account Account { get; private set; }

        public void SetAccount(Account account)
        {
            this.Account = account;
        }

        public void ChangeStatus(AccountStatus status)
        {
            if (this.Status == AccountStatus.Closed)
            {
                throw new InvalidObjectException(nameof(AccountStatus), "You cannot change the status of the closed acccount");
            }

            this.Status = status;
        }

        public void SetAccountPerformance(AccountPerformance performance)
        {
            this.Performance = performance;
        }
    }
}
