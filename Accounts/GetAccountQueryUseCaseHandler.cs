using MoreLinq.Extensions;
using Sumday.Domain.Abstractions.EntryPorts;
using Sumday.Domain.Abstractions.ExitPorts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public class GetAccountQueryUseCaseHandler : IQueryUseCaseHandler<GetAccountQuery, AccountInstance>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEnumerable<ISetAndGetAccountOwnerWithBeneficiary> setAndGetAccountBeneficiaries;

        public GetAccountQueryUseCaseHandler(IUnitOfWork unitOfWork, IEnumerable<ISetAndGetAccountOwnerWithBeneficiary> setAndGetAccountBeneficiaries)
        {
            this.unitOfWork = unitOfWork;
            this.setAndGetAccountBeneficiaries = setAndGetAccountBeneficiaries;
        }

        public async Task<UseCaseExecutionResult> Handle(QueryUseCase<GetAccountQuery, AccountInstance> request, CancellationToken cancellationToken)
        {
            var accountSpecification = new GetAccountSpecification(request.Query.AccountNumber);

            var account = await this.unitOfWork.Repository<AccountInstance>().Get(accountSpecification, cancellationToken);

            account = await this.setAndGetAccountBeneficiaries.FirstOrDefault(y => y.PlanType == account.PlanId.Type)
                                .AggregateWithEntities(accountSpecification, cancellationToken);

            if (!accountSpecification.IsSatisfied.Successfully)
            {
                request.OutputPort.Output(UseCaseResult<AccountInstance>.ExecutionFail(accountSpecification.ExecutionResultNotification.Errors));
                return UseCaseExecutionResult.Failure;
            }

            request.OutputPort.Output(UseCaseResult<AccountInstance>.Success(account));
            return UseCaseExecutionResult.Success;
        }
    }
}
