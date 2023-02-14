using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    public class AddOneModel<TAggregate> : ChangeModel<TAggregate>
          where TAggregate : AggregateRoot
    {
        private readonly TAggregate aggregate;

        public AddOneModel(TAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public TAggregate Aggregate
        {
            get { return this.aggregate; }
        }

        public override ChangeModelType ModelType
        {
            get { return ChangeModelType.AddOne; }
        }
    }
}
