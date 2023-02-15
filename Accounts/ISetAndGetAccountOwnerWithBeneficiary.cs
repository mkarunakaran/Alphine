using Sumday.BoundedContext.SharedKernel;
using Sumday.BoundedContext.SharedKernel.ValueObjects;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public interface ISetAndGetAccountOwnerWithBeneficiary : ISetAndGetBasedonSpecification<AccountInstance, GetAccountSpecification>
    {
        PlanType PlanType { get; }
    }
}
