using System.Threading;
using System.Threading.Tasks;
using Sumday.Infrastructure.Common.ChangeTracking;

namespace Sumday.Infrastructure.Common
{
    public abstract class BaseRepository
    {
        internal abstract Task CommitChanges(BasePreparedChangeModel writeModel, CancellationToken cancellationToken = default);

        internal abstract BasePreparedChangeModel PrepareChangesForWrite();
    }
}
