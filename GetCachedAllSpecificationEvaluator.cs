using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ObjectCloner.Extensions;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;

namespace Sumday.Infrastructure.Surpas
{
    public class GetCachedAllSpecificationEvaluator<TAggregate, TAllSpecification> : GetAllSpecificationEvaluator<TAggregate, TAllSpecification>, IAllCacheSpecificationEvaluator<TAggregate, TAllSpecification>
        where TAggregate : AggregateRoot
        where TAllSpecification : IAllSpecification<TAggregate>
    {
        public GetCachedAllSpecificationEvaluator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual bool ByPassCache { get; set; }

        public List<CacheEnvelope<TAggregate>> CacheEnvelopes { get; set; } = new List<CacheEnvelope<TAggregate>>();

        public override async Task<IReadOnlyList<TAggregate>> GetAll(TAllSpecification specification, CancellationToken cancellationToken = default)
        {
            var baseSpecification = specification as BaseAllSpecification<TAggregate>;
            var aggregates = new List<TAggregate>();
            if (this.ByPassCache || !this.CacheEnvelopes.Any())
            {
                this.CacheEnvelopes = new List<CacheEnvelope<TAggregate>>();
                aggregates = await this.GetPagedAggregates(specification, cancellationToken);
                if (baseSpecification.IsSatisfied.Successfully)
                {
                    aggregates.ForEach(agg =>
                    {
                        var cachedenvelope = new CacheEnvelope<TAggregate>() { Aggregate = agg.DeepClone(), Metadata = this.Metadata };
                        this.CacheEnvelopes.Add(cachedenvelope);
                    });
                }
            }
            else
            {
                aggregates = this.CacheEnvelopes.Select(env => env.Aggregate).ToList();
                this.Metadata = this.CacheEnvelopes.FirstOrDefault().Metadata;

                if (baseSpecification.IsPagingEnabled && aggregates.Count > (baseSpecification.Skip * baseSpecification.Take))
                {
                    aggregates = this.ApplyOperations(specification, aggregates);
                }
                else
                {
                    if (this.Metadata.ContainsKey(SurpasConstants.Context) && !string.IsNullOrEmpty(SurpasConstants.Context))
                    {
                        aggregates = await this.GetPagedAggregates(specification, cancellationToken);
                        aggregates.ForEach(agg =>
                        {
                            var cachedenvelope = new CacheEnvelope<TAggregate>() { Aggregate = agg.DeepClone(), Metadata = this.Metadata };
                            this.CacheEnvelopes.Add(cachedenvelope);
                        });
                    }
                }
            }

            return aggregates.AsReadOnly();
        }
    }
}
