using Sumday.BoundedContext.ShareHolder.Shared.ValueObjects;
using Sumday.Domain.Abstractions.EntryPorts;

namespace Sumday.BoundedContext.ShareHolder
{
    public class GetAllAccountsQuery : IQueryInputPort
    {
        public GetAllAccountsQuery(string sharedHolderId)
        {
            this.SharedHolderId = sharedHolderId;
        }

        public string SharedHolderId { get; }
    }
}
