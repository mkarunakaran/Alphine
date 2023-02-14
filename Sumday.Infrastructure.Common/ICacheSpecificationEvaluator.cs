using System;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;

namespace Sumday.Infrastructure.Common
{
    public interface ICacheSpecificationEvaluator<TAggregate, TSpecification>
         where TAggregate : AggregateRoot
         where TSpecification : ISpecification<TAggregate>
    {
        bool ByPassCache { set; }

        CacheEnvelope<TAggregate> CacheEnvelope { get; set; }
    }
}
