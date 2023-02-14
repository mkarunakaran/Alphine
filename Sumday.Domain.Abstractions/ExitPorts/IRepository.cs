using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IRepository<TAggregate>
        where TAggregate : AggregateRoot
    {
        Task<TAggregate> Get(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TAggregate>> GetAll(IAllSpecification<TAggregate> specification, CancellationToken cancellationToken = default);

        void Add(TAggregate entity);

        void Delete(TAggregate entity);
    }
}
