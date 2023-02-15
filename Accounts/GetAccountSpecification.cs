using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts
{
    public class GetAccountSpecification : BaseSpecification<AccountInstance>
    {
        public GetAccountSpecification(string accouuntnumber)
        {
            this.AccountNumber = accouuntnumber;
        }

        [EntityKey]
        public string AccountNumber { get; }
    }
}
