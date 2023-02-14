using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;

namespace Sumday.Infrastructure.Common
{
    public interface IGetAllSpecificationEvaluator<TAggregate, TAllSpecification>
        where TAggregate : AggregateRoot
        where TAllSpecification : IAllSpecification<TAggregate>
    {
        Task<IReadOnlyList<TAggregate>> GetAll(TAllSpecification specification, CancellationToken cancellationToken = default);
    }
}
