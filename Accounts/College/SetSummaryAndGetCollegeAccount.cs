using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.BoundedContext.ShareHolder.Accounts.College;
using Sumday.Domain.Abstractions.ExitPorts;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able
{
    public class SetSummaryAndGetCollegeAccount : ISetAndGetAccountOwnerWithBeneficiary
    {
        private readonly IUnitOfWork unitOfWork;

        public SetSummaryAndGetCollegeAccount(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PlanType PlanType => PlanType.CollegeSavings;

        public Task<AccountInstance> AggregateWithEntities(GetAccountSpecification specification, CancellationToken cancellationToken = default)
        {
            specification.Include<Account>(x => x.Account as CollegeAccount)
                .ThenInclude<CollegeAccount>(act => act.Include(clgAccount => clgAccount.Beneficiary));

            var account = this.unitOfWork.Repository<AccountInstance>().Get(specification, cancellationToken);
            if (!specification.IsSatisfied.Successfully)
            {
                return null;
            }

            return account;
        }
    }
}
