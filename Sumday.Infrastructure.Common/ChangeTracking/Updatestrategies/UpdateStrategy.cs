using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies
{
    internal abstract class UpdateStrategy<TAggregate> : BaseUpdateStrategy
        where TAggregate : AggregateRoot
    {
        public abstract ChangeModel<TAggregate> GetWriteModelForUpdate(TrackedModel<TAggregate> trackedModel);
    }
}
