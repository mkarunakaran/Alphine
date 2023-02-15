using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Domain.Abstractions.ExitPorts;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able
{
    public class SetSummaryAndGetAbleAccount : ISetAndGetAccountOwnerWithBeneficiary
    {
        private readonly IUnitOfWork unitOfWork;

        public SetSummaryAndGetAbleAccount(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PlanType PlanType => PlanType.Able;

        public Task<AccountInstance> AggregateWithEntities(GetAccountSpecification specification, CancellationToken cancellationToken = default)
        {
            specification.Include<Account>(x => x.Account as AbleAccount)
                .ThenInclude<AbleAccount>(act => act.Include(abAccount => abAccount.Beneficiary));

            var account = this.unitOfWork.Repository<AccountInstance>().Get(specification, cancellationToken);
            if (!specification.IsSatisfied.Successfully)
            {
                return null;
            }

            return account;
        }
    }
}
