using Sumday.Domain.Abstractions.EntryPorts;

namespace Sumday.BoundedContext.ShareHolder
{
    public class GetAccountQuery : IQueryInputPort
    {
        public GetAccountQuery(string accountNumber)
        {
            this.AccountNumber = accountNumber;
        }

        public string AccountNumber { get; }
    }
}
