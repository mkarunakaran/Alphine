using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;
using Sumday.Infrastructure.Common.Caching;
using Sumday.Infrastructure.Common.ChangeTracking;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Surpas
{
    public class AggregateCollection<TAggregate> : IAggregateCollection<TAggregate>
        where TAggregate : AggregateRoot
    {
        private const string ByPassCache = "ByPassCache";
        private readonly IServiceProvider serviceProvider;

        public AggregateCollection(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<ExecutionResult> Commit(IEnumerable<ChangeModel<TAggregate>> writeModels, CancellationToken cancellationToken = default)
        {
            var callContext = this.serviceProvider.GetRequiredService<ICallContext>();
            var simulate = callContext[SurpasConstants.Simulate] as bool?;
            var aggregateService = this.serviceProvider.GetRequiredService<IAggregateService<TAggregate>>();
            foreach (var writeModel in writeModels)
            {
                var aggregateEnvelope = new CacheEnvelope<TAggregate>();
                if (writeModel.ModelType == ChangeModelType.AddOne)
                {
                    var addOneModel = (AddOneModel<TAggregate>)writeModel;
                    await aggregateService.Add(addOneModel.Aggregate,  simulate.GetValueOrDefault(false), cancellationToken);
                }

                if (writeModel.ModelType == ChangeModelType.UpdateOne)
                {
                    var updateOneModel = (UpdateOneModel<TAggregate>)writeModel;
                    await aggregateService.Add(updateOneModel.Aggregate, simulate.GetValueOrDefault(false), cancellationToken);
                  ////  aggregateEnvelope.Aggregate = updateOneModel.Aggregate;
                 ////   await this.InvalidateOrUpdate(aggregateEnvelope, cancellationToken);
                }

                if (writeModel.ModelType == ChangeModelType.DeleteOne)
                {
                    var deleteOneModel = (DeleteOneModel<TAggregate>)writeModel;
                    await aggregateService.Add(deleteOneModel.Aggregate,  simulate.GetValueOrDefault(false), cancellationToken);
                   //// aggregateEnvelope.Aggregate = deleteOneModel.Aggregate;
                  ////  await this.InvalidateOrUpdate(aggregateEnvelope, cancellationToken);
                }
            }

            return new ExecutionResult();
        }

        public async Task<TAggregate> GetAggregate(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var aggregate = default(TAggregate);
            var callContext = this.serviceProvider.GetRequiredService<ICallContext>();
            var byPassCache = callContext[SurpasConstants.ByPassCache] as bool?;
            var cacheProvider = this.serviceProvider.GetRequiredService<ICacheProvider>();
            var specificationType = specification.GetType();
            var evaluatorType = typeof(IGetSpecificationEvaluator<,>);
            Type[] typeArgs = { typeof(TAggregate), specificationType };
            Type constructed = evaluatorType.MakeGenericType(typeArgs);
            var evaluator = this.serviceProvider.GetService(constructed);
            var method = evaluator.GetType().GetMethod("Get");
            if (evaluator.GetType().Implements(typeof(ICacheSpecificationEvaluator<,>)))
            {
                var aggregateName = typeof(TAggregate).Name;
                var aggregateCaches = await cacheProvider.Get<List<CacheAggregate<TAggregate>>>(aggregateName, cancellationToken);
                aggregateCaches ??= new List<CacheAggregate<TAggregate>>();

                evaluator.SetPropValue(ByPassCache, byPassCache.GetValueOrDefault(false));
                var specificationKey = specification.GetObjectKey();
                var cachedAggregate = aggregateCaches.SingleOrDefault(ch => specificationKey == ch.GetSpecificationKey);
                if (cachedAggregate != null && cachedAggregate.CacheEnvelope != null)
                {
                    evaluator.SetPropValue("CacheEnvelope", cachedAggregate.CacheEnvelope);
                }

                aggregate = await method.InvokeAsync<TAggregate>(evaluator, new object[] { specification, cancellationToken });

                if (aggregate != null)
                {
                    var cachedEnvelope = evaluator.GetPropValue("CacheEnvelope") as CacheEnvelope<TAggregate>;
                    var aggregateKey = aggregate.GetEntityKey();
                    if (cachedAggregate == null)
                    {
                        cachedAggregate = new CacheAggregate<TAggregate>() { AggregateKey = aggregateKey, GetSpecificationKey = specificationKey, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now };
                        aggregateCaches.Add(cachedAggregate);
                    }

                    cachedAggregate.CacheEnvelope = cachedEnvelope;
                    cachedAggregate.LastModifiedDate = DateTime.UtcNow;
                }

                aggregateCaches.RemoveAll(aggCache => aggCache.LastModifiedDate.Subtract(DateTime.UtcNow).TotalMinutes > 30);
                await cacheProvider.Set(aggregateName, aggregateCaches, TimeSpan.FromHours(1), true, cancellationToken);
            }
            else
            {
                aggregate = await method.InvokeAsync<TAggregate>(evaluator, new object[] { specification, cancellationToken });
            }

            return aggregate;
        }

        public async Task<IReadOnlyList<TAggregate>> GetAllAggregates(IAllSpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var aggregatesFromCall = new List<TAggregate>().AsReadOnly() as IReadOnlyList<TAggregate>;
            var cacheProvider = this.serviceProvider.GetRequiredService<ICacheProvider>();
            var callContext = this.serviceProvider.GetRequiredService<ICallContext>();
            var byPassCache = callContext[SurpasConstants.ByPassCache] as bool?;
            var specificationType = specification.GetType();
            var evaluatorType = typeof(IGetAllSpecificationEvaluator<,>);
            Type[] typeArgs = { typeof(TAggregate), specificationType };
            Type constructed = evaluatorType.MakeGenericType(typeArgs);
            var evaluator = this.serviceProvider.GetService(constructed);
            var method = evaluator.GetType().GetMethod("GetAll");
            if (evaluator.GetType().Implements(typeof(IAllCacheSpecificationEvaluator<,>)))
            {
                var aggregateName = typeof(TAggregate).Name;
                var aggregateCaches = await cacheProvider.Get<List<CacheAggregate<TAggregate>>>(aggregateName, cancellationToken);
                aggregateCaches ??= new List<CacheAggregate<TAggregate>>();

                var specificationKey = specification.GetType().Name + specification.GetObjectKey();
                var currentAggregates = aggregateCaches.Where(agg => agg.GetAllSpecificationKey.Equals(specificationKey)).ToList();
                var cacheEnvelopes = currentAggregates.Where(agg => agg != null).Select(agg => agg.CacheEnvelope).ToList();
                if (cacheEnvelopes != null && cacheEnvelopes.Any())
                {
                    evaluator.SetPropValue("CacheEnvelopes", cacheEnvelopes);
                }

                evaluator.SetPropValue(ByPassCache, byPassCache.GetValueOrDefault(false));
                aggregatesFromCall = await method.InvokeAsync<IReadOnlyList<TAggregate>>(evaluator, new object[] { specification, cancellationToken });
                cacheEnvelopes = evaluator.GetPropValue("CacheEnvelopes") as List<CacheEnvelope<TAggregate>>;
                cacheEnvelopes.ForEach(env =>
                {
                    var aggregateKey = env.Aggregate.GetEntityKey();
                    var aggregateKeyName = env.Aggregate.GetEntityKeyWithName();
                    var cachedAggregate = currentAggregates.FirstOrDefault(agExist => agExist != null && agExist.AggregateKey.Equals(aggregateKey));
                    if (cachedAggregate == null)
                    {
                        cachedAggregate = new CacheAggregate<TAggregate>() { AggregateKey = aggregateKey, GetSpecificationKey = aggregateKeyName, GetAllSpecificationKey = specificationKey, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, CacheEnvelope = env };
                        aggregateCaches.Add(cachedAggregate);
                    }

                    cachedAggregate.LastModifiedDate = DateTime.UtcNow;
                });
                aggregateCaches.RemoveAll(aggCache => aggCache.LastModifiedDate.Subtract(DateTime.UtcNow).TotalMinutes > 30);
                await cacheProvider.Set(aggregateName, aggregateCaches, TimeSpan.FromHours(1), true, cancellationToken);
            }
            else
            {
                aggregatesFromCall = await method.InvokeAsync<IReadOnlyList<TAggregate>>(evaluator, new object[] { specification, cancellationToken });
            }

            return aggregatesFromCall;
        }

        ////public async Task InvalidateOrUpdate(CacheEnvelope<TAggregate> aggregateEnvelope, CancellationToken cancellationToken, bool update = false)
        ////{
        ////    var cacheProvider = this.serviceProvider.GetRequiredService<ICacheProvider>();
        ////    var key = aggregateEnvelope.Aggregate.GetEntityKey();
        ////    var cacheKey = string.Concat(typeof(TAggregate).Name, "|", key.ToString());
        ////    await cacheProvider.Remove(cacheKey, cancellationToken);
        ////    if (update)
        ////    {
        ////        await cacheProvider.Set(cacheKey, aggregateEnvelope, TimeSpan.FromHours(1), true, cancellationToken);
        ////    }
        ////}
    }
}
