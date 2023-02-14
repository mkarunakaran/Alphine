using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common.ChangeTracking;

namespace Sumday.Infrastructure.Common
{
    public interface IAggregateCollection<TAggregate>
        where TAggregate : AggregateRoot
    {
        Task<TAggregate> GetAggregate(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TAggregate>> GetAllAggregates(IAllSpecification<TAggregate> specification, CancellationToken cancellationToken = default);

        Task<ExecutionResult> Commit(IEnumerable<ChangeModel<TAggregate>> writeModels, CancellationToken cancellationToken = default);
    }
}
