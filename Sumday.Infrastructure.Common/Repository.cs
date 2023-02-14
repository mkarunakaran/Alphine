using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common.ChangeTracking;

namespace Sumday.Infrastructure.Common
{
    public class Repository<TAggregate> : BaseRepository, IRepository<TAggregate>
        where TAggregate : AggregateRoot
    {
        private readonly TrackedModelCollection<TAggregate> trackedModels = new TrackedModelCollection<TAggregate>();
        private readonly IAggregateCollection<TAggregate> aggregateCollection;

        public Repository(IAggregateCollection<TAggregate> aggregateCollection)
        {
            this.aggregateCollection = aggregateCollection;
        }

        public void Add(TAggregate entity)
        {
            this.trackedModels.New(entity);
        }

        public void Delete(TAggregate entity)
        {
            this.trackedModels.Remove(entity);
        }

        public async Task<TAggregate> Get(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var result = await this.aggregateCollection.GetAggregate(specification, cancellationToken);

            if (result != null)
            {
                this.trackedModels.Existing(result);
            }

            return result;
        }

        public async Task<IReadOnlyList<TAggregate>> GetAll(IAllSpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var results = await this.aggregateCollection.GetAllAggregates(specification, cancellationToken);

            foreach (var result in results)
            {
                this.trackedModels.Existing(result);
            }

            return results;
        }

        internal override async Task<ExecutionResult> CommitChanges(BasePreparedChangeModel writeModel, CancellationToken cancellationToken = default)
        {
            var typedWriteModel = (PreparedChangeModel<TAggregate>)writeModel;
            if (typedWriteModel.HasChanges)
            {
               return await TrackedModelPersister<TAggregate>.PersistChanges(this.aggregateCollection, typedWriteModel.AggregateWriteModels, cancellationToken);
            }

            return new ExecutionResult();
        }

        internal override BasePreparedChangeModel PrepareChangesForWrite() => new PreparedChangeModel<TAggregate>(TrackedModelPersister<TAggregate>.GetChangesForWrite(this.trackedModels));
    }
}
