using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;

namespace Sumday.Infrastructure.Surpas
{
    public interface IAggregateService<TAggregate>
        where TAggregate : AggregateRoot
    {
        Task<TAggregate> Get(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default) => Task.FromResult(default(TAggregate));

        Task<List<TAggregate>> GetAll(IAllSpecification<TAggregate> specification, Dictionary<string, object> metadata, CancellationToken cancellationToken = default) => Task.FromResult(new List<TAggregate>());

        Task<ExecutionResult> Add(TAggregate aggregate, bool simulate = false, CancellationToken cancellationToken = default) => Task.FromResult(new ExecutionResult());

        Task<ExecutionResult> Update(TAggregate aggregate, bool simulate = false, CancellationToken cancellationToken = default) => Task.FromResult(new ExecutionResult());

        Task<ExecutionResult> Delete(TAggregate aggregate, bool simulate = false, CancellationToken cancellationToken = default) => Task.FromResult(new ExecutionResult());
    }
}
