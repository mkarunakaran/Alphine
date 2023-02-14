using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;

namespace Sumday.Infrastructure.Common
{
    public interface IGetSpecificationEvaluator<TAggregate, TSpecification>
         where TAggregate : AggregateRoot
         where TSpecification : ISpecification<TAggregate>
    {
        Task<TAggregate> Get(TSpecification specification, CancellationToken cancellationToken = default);
    }
}
