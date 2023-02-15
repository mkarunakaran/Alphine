using MoreLinq.Extensions;
using Sumday.Domain.Abstractions.EntryPorts;
using Sumday.Domain.Abstractions.ExitPorts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public class GetAllAccountsQueryUseCaseHandler : IQueryUseCaseHandler<GetAllAccountsQuery, List<AccountInstance>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IEnumerable<ISetAndGetAccountOwnerWithBeneficiary> setAndGetAccountBeneficiaries;

        public GetAllAccountsQueryUseCaseHandler(IUnitOfWork unitOfWork, IEnumerable<ISetAndGetAccountOwnerWithBeneficiary> setAndGetAccountBeneficiaries)
        {
            this.unitOfWork = unitOfWork;
            this.setAndGetAccountBeneficiaries = setAndGetAccountBeneficiaries;
        }

        public async Task<UseCaseExecutionResult> Handle(QueryUseCase<GetAllAccountsQuery, List<AccountInstance>> request, CancellationToken cancellationToken)
        {
            var shareHolderSpecification = new GetShareHolderSpecification(request.Query.SharedHolderId);

            var shareHolderInstance = await this.unitOfWork.Repository<ShareHolderInstance>().Get(shareHolderSpecification, cancellationToken);

            var allAccountsSpecification = new GetAllAccountsSpecification(shareHolderInstance.ShareHolder.ShareHolderIdentity.Tin);

            var accountResults = await this.unitOfWork.Repository<AccountInstance>().GetAll(allAccountsSpecification, cancellationToken);

            if (!allAccountsSpecification.IsSatisfied.Successfully)
            {
                request.OutputPort.Output(UseCaseResult<List<AccountInstance>>.ExecutionFail(allAccountsSpecification.ExecutionResultNotification.Errors));
                return UseCaseExecutionResult.Failure;
            }

            var accountReqs = accountResults.Batch(100);
            var accounts = new List<AccountInstance>();
            foreach (var result in accountReqs)
            {
                var partialAccounts = await Task.WhenAll(result.Select(x => this.setAndGetAccountBeneficiaries.FirstOrDefault(y => y.PlanType == x.PlanId.Type)
                .AggregateWithEntities(new GetAccountSpecification(x.AccountNumber), cancellationToken)));

                accounts.AddRange(partialAccounts.Where(x => x != null && x.AccountNumber != null));
            }

            request.OutputPort.Output(UseCaseResult<List<AccountInstance>>.Success(accounts));
            return UseCaseExecutionResult.Success;
        }
    }
}
