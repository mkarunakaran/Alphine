using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Domain.Abstractions.ExitPorts;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able
{
    public class SetSummaryAndGetSecureAccount : ISetAndGetAccountOwnerWithBeneficiary
    {
        private readonly IUnitOfWork unitOfWork;

        public SetSummaryAndGetSecureAccount(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PlanType PlanType => PlanType.SecureChoice;

        public Task<AccountInstance> AggregateWithEntities(GetAccountSpecification specification, CancellationToken cancellationToken = default)
        {
            specification.Include<Account>(x => x.Account as SecureChoiceAccount)
                .ThenInclude<SecureChoiceAccount>(act => act.Include(sec => sec.Beneficiary));
            var account = this.unitOfWork.Repository<AccountInstance>().Get(specification, cancellationToken);
            if (!specification.IsSatisfied.Successfully)
            {
                return null;
            }

            return account;
        }
    }
}
