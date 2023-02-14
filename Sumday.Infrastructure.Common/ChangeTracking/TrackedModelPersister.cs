using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class TrackedModelPersister<TAggregate>
          where TAggregate : AggregateRoot
    {
        public static async Task<ExecutionResult> PersistChanges(IAggregateCollection<TAggregate> collection,  IEnumerable<ChangeModel<TAggregate>> changedModels, CancellationToken cancellationToken = default)
        {
           return await collection.Commit(changedModels, cancellationToken);
        }

        public static IEnumerable<ChangeModel<TAggregate>> GetChangesForWrite(TrackedModelCollection<TAggregate> trackedModels)
        {
            return InsertNewModels(trackedModels)
                .Concat(DeleteRemovedModels(trackedModels))
                .Concat(UpdateChangedModels(trackedModels));
        }

        private static IEnumerable<ChangeModel<TAggregate>> InsertNewModels(TrackedModelCollection<TAggregate> trackedModels)
        {
            var newModels = trackedModels.OfState(TrackedModelState.New).Select(m => m.Model).ToArray();
            return newModels.Select(m => new AddOneModel<TAggregate>(m));
        }

        private static IEnumerable<ChangeModel<TAggregate>> DeleteRemovedModels(TrackedModelCollection<TAggregate> trackedModels)
        {
            var removedModels = trackedModels.OfState(TrackedModelState.Removed).Select(m => m.Model).ToArray();
            return removedModels.Select(m => new DeleteOneModel<TAggregate>(m));
        }

        private static IEnumerable<ChangeModel<TAggregate>> UpdateChangedModels(TrackedModelCollection<TAggregate> trackedModels)
        {
            var updatedModels = trackedModels.OfState(TrackedModelState.Existing).Where(m => m.HasChange).ToArray();
            var updateStrategy = BaseUpdateStrategy.ForType<TAggregate>();
            return updatedModels.Select(m => updateStrategy.GetWriteModelForUpdate(m));
        }
    }
}
