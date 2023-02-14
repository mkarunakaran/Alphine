using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    public class DeleteOneModel<TAggregate> : ChangeModel<TAggregate>
         where TAggregate : AggregateRoot
    {
        private readonly TAggregate aggregate;

        public DeleteOneModel(TAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public TAggregate Aggregate
        {
            get { return this.aggregate; }
        }

        public override ChangeModelType ModelType => ChangeModelType.DeleteOne;
    }
}
