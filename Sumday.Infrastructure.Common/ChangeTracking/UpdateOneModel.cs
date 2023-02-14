using Sumday.Domain.Abstractions;
using Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    public class UpdateOneModel<TAggregate> : ChangeModel<TAggregate>
         where TAggregate : AggregateRoot
    {
        private readonly TAggregate aggregate;

        private readonly UpdateDefinition updateDefinition;

        public UpdateOneModel(TAggregate aggregate, UpdateDefinition updateDefinition)
        {
            this.aggregate = aggregate;
            this.updateDefinition = updateDefinition;
        }

        public TAggregate Aggregate
        {
            get { return this.aggregate; }
        }

        public UpdateDefinition UpdateDefinition
        {
            get { return this.updateDefinition; }
        }

        public override ChangeModelType ModelType => ChangeModelType.UpdateOne;
    }
}
