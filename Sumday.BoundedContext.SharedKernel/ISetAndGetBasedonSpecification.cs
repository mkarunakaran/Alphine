using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.SharedKernel
{
     public interface ISetAndGetBasedonSpecification<TAggregate, TSpecification>
        where TAggregate : AggregateRoot
        where TSpecification : ISpecification<TAggregate>
    {
        Task<TAggregate> AggregateWithEntities(TSpecification specification, CancellationToken cancellationToken = default);
    }
}
