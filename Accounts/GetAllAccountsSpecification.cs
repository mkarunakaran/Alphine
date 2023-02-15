using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public class GetAllAccountsSpecification : BaseAllSpecification<AccountInstance>
    {
        public GetAllAccountsSpecification(Tin tin)
        {
            this.Tin = tin;
        }

        public Tin Tin { get; }
    }
}
