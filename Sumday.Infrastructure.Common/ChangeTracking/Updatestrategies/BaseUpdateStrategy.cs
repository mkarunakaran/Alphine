using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies
{
    internal abstract class BaseUpdateStrategy
    {
        public static UpdateStrategy<TAggregate> ForType<TAggregate>()
            where TAggregate : AggregateRoot
        {
            return new DeltaUpdateStrategy<TAggregate>();
        }
    }
}
