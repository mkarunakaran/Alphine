using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal abstract class BasePreparedChangeModel
    {
        protected BasePreparedChangeModel(object internalChangeModel)
        {
            this.InternalChangeModel = internalChangeModel;
        }

        protected object InternalChangeModel { get; }

        public PreparedChangeModel<TAggregate> Cast<TAggregate>()
            where TAggregate : AggregateRoot
        {
            return (PreparedChangeModel<TAggregate>)this;
        }
    }
}
