using Sumday.Domain.Abstractions.EntryPorts;

namespace Sumday.BoundedContext.ShareHolder
{
    public class GetSharedHolderProfileQuery : IQueryInputPort
    {
        public GetSharedHolderProfileQuery(string sharedHolderId)
        {
            this.SharedHolderId = sharedHolderId;
        }

        public string SharedHolderId { get; }
    }
}
