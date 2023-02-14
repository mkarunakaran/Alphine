using System;
using System.Linq;
using Sumday.Domain.Abstractions;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies
{
    internal class DeltaUpdateStrategy<TAggregate> : UpdateStrategy<TAggregate>
        where TAggregate : AggregateRoot
    {
        public override ChangeModel<TAggregate> GetWriteModelForUpdate(TrackedModel<TAggregate> trackedModel)
        {
            var updateDefinition = this.GetUpdateDefinitionForObject(trackedModel.ChangeTracker, trackedModel.Model);

            return new UpdateOneModel<TAggregate>(trackedModel.Model, updateDefinition);
        }

        private DeltaUpdateDefinition GetUpdateDefinitionForObject(IObjectChangeTracker dirtyTracker, object aggregate)
        {
            var updateDefinition = new DeltaUpdateDefinition();
            foreach (var memberTracker in dirtyTracker.MemberTrackers.Where(t => t.HasChange))
            {
                var objectTracker = (IObjectChangeTracker)memberTracker;

                var model = aggregate.GetPropValue(memberTracker.ElementName);

                if (model == null)
                {
                    updateDefinition.Set(memberTracker.ElementName, null);
                    continue;
                }

                if (memberTracker.OriginalValue == null)
                {
                    updateDefinition.Set(memberTracker.ElementName, memberTracker.CurrentValue);
                    continue;
                }

                var subUpdateDefinition = this.GetUpdateDefinitionForObject(objectTracker, model);
                updateDefinition.Merge(memberTracker.ElementName, subUpdateDefinition);
            }

            return updateDefinition;
        }
    }
}
