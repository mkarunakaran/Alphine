using System.Collections.Generic;
using Sumday.Domain.Abstractions;

namespace Sumday.Infrastructure.Common
{
    public class CacheEnvelope<TAggregate>
         where TAggregate : AggregateRoot
    {
        public CacheEnvelope()
        {
            this.Includes = new Dictionary<string, Entity>();
            this.Metadata = new Dictionary<string, object>();
        }

        public TAggregate Aggregate { get; set; }

        public Dictionary<string, Entity> Includes { get; set; }

        public Dictionary<string, object> Metadata { get; set; }
    }
}
