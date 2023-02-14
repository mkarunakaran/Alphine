using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    public class DeleteManyModel<TAggregate> : ChangeModel<TAggregate>
         where TAggregate : AggregateRoot
    {
        private readonly TAggregate aggregate;

        public DeleteManyModel(TAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public TAggregate Aggregate
        {
            get { return this.aggregate; }
        }

        public override ChangeModelType ModelType => ChangeModelType.DeleteMany;
    }
}
