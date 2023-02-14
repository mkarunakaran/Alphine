using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    public abstract class ChangeModel<TAggregate>
              where TAggregate : AggregateRoot
    {
         public abstract ChangeModelType ModelType { get; }
    }
}
