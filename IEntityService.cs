using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;

namespace Sumday.Infrastructure.Surpas
{
    public interface IEntityService<TAggregate, TEntity>
        where TAggregate : AggregateRoot
        where TEntity : Entity
    {
        Task<TEntity> GetAndSet(TAggregate aggregate, ISpecification<TAggregate> specification, CancellationToken cancellationToken = default) => Task.FromResult(default(TEntity));

        Task<ExecutionResult> Add(TEntity entity, bool simulate = false, CancellationToken cancellationToken = default) => Task.FromResult(new ExecutionResult());

        Task<ExecutionResult> Update(TEntity entity,  bool simulate = false, CancellationToken cancellationToken = default) => Task.FromResult(new ExecutionResult());

        Task<ExecutionResult> Delete(TEntity entity, bool simulate = false, CancellationToken cancellationToken = default) => Task.FromResult(new ExecutionResult());
    }
}
