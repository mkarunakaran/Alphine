using System;
using System.Collections.Generic;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;

namespace Sumday.Infrastructure.Common
{
    public interface IAllCacheSpecificationEvaluator<TAggregate, TAllSpecification> : ICachedSpecification
         where TAggregate : AggregateRoot
         where TAllSpecification : IAllSpecification<TAggregate>
    {
        List<CacheEnvelope<TAggregate>> CacheEnvelopes { get; set; }
    }
}
