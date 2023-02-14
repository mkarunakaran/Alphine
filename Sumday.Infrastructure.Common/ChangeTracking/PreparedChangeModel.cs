using System.Collections.Generic;
using System.Linq;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal class PreparedChangeModel<TAggregate> : BasePreparedChangeModel
          where TAggregate : AggregateRoot
    {
        public PreparedChangeModel(IEnumerable<ChangeModel<TAggregate>> internalChangeModel)
            : base(internalChangeModel)
        {
        }

        public bool HasChanges => this.AggregateWriteModels.Any();

        public IEnumerable<ChangeModel<TAggregate>> AggregateWriteModels => (IEnumerable<ChangeModel<TAggregate>>)this.InternalChangeModel;
    }
}
