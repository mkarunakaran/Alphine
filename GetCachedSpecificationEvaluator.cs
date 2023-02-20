using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ObjectCloner.Extensions;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;
using Sumday.Infrastructure.Extensions;
using ValidationException = Sumday.BoundedContext.SharedKernel.Exceptions.ValidationException;

namespace Sumday.Infrastructure.Surpas
{
    public class GetCachedSpecificationEvaluator<TAggregate, TSpecification> : GetSpecificationEvaluator<TAggregate, TSpecification>, ICacheSpecificationEvaluator<TAggregate, TSpecification>
         where TAggregate : AggregateRoot
         where TSpecification : ISpecification<TAggregate>
    {
        private readonly IServiceProvider serviceProvider;

        public GetCachedSpecificationEvaluator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public CacheEnvelope<TAggregate> CacheEnvelope { get; set; } = new CacheEnvelope<TAggregate>();

        public virtual bool ByPassCache { get; set; }

        public override async Task<TAggregate> Get(TSpecification specification, CancellationToken cancellationToken = default)
        {
            var baseSpecification = specification as BaseSpecification<TAggregate>;

            var aggregate = default(TAggregate);
            var aggregateInCache = this.CacheEnvelope.Aggregate;
            if (this.ByPassCache || aggregateInCache == null)
            {
                this.CacheEnvelope = new CacheEnvelope<TAggregate>();
                var aggregateService = this.serviceProvider.GetRequiredService<IAggregateService<TAggregate>>();
                try
                {
                    aggregate = await aggregateService.Get(specification, cancellationToken);
                }
                catch (ValidationException ex)
                {
                    ex.Errors.ForEach(err => baseSpecification.Notification.AddError(err));
                }
                catch (DomainException ex)
                {
                    baseSpecification.Notification.AddError(new ExecutionError(ex.Message, ex.FieldName, ExecutionErrorType.DomainValidation));
                }
                catch (SumdaySupportCodeException ex)
                {
                    baseSpecification.Notification.AddError(new ExecutionError(ex.Message, ex.SupportCode.ToString(), ExecutionErrorType.SystemSupport));
                }
                catch (Exception ex)
                {
                    baseSpecification.Notification.AddError(new ExecutionError(ex.Message, string.Empty, ExecutionErrorType.General));
                }

                if (baseSpecification.IsSatisfied.Successfully)
                {
                    var copiedAgregate = aggregate.DeepClone();
                    this.CacheEnvelope.Aggregate = aggregate;
                    this.CacheEnvelope.Includes = await this.PopulateEntities(aggregate, specification, cancellationToken);
                }
            }
            else
            {
                aggregate = aggregateInCache.DeepClone();
                var pathTasks = new Dictionary<string, object>();

                var memberObjects = new Dictionary<string, Type>();

                foreach (var navigationStringPath in baseSpecification.IncludeStrings)
                {
                    var pathInfo = typeof(TAggregate).GetPropInfo(navigationStringPath);
                    memberObjects.Add(navigationStringPath, pathInfo.PropertyType);
                }

                foreach (var navigationPropertyPath in baseSpecification.Includes)
                {
                    var paths = navigationPropertyPath.PropertyPath();
                    paths.ForEach(p => memberObjects.Add(p.Key.Replace($"{typeof(TAggregate).Name}.", string.Empty), p.Value));
                }

                foreach (var childPath in memberObjects)
                {
                    var memeberType = childPath.Value;
                    if (typeof(Entity).IsAssignableFrom(memeberType) && !this.CacheEnvelope.Includes.ContainsKey(childPath.Key))
                    {
                        var entityType = typeof(IEntityService<,>);
                        Type[] typeArgs = { typeof(TAggregate), memeberType };
                        Type constructed = entityType.MakeGenericType(typeArgs);
                        var entityService = this.serviceProvider.GetService(constructed);
                        if (entityService != null)
                        {
                            var method = constructed.GetMethod("GetAndSet");

                            var entity = await method.InvokeAsync<object>(entityService, new object[] { aggregate, specification, cancellationToken });

                            pathTasks.Add(childPath.Key, entity);
                        }
                    }
                    else
                    {
                        if (this.CacheEnvelope.Includes.ContainsKey(childPath.Key))
                        {
                            aggregate.SetPropValue(childPath.Key, this.CacheEnvelope.Includes[childPath.Key]);
                        }
                    }
                }

                if (pathTasks.Count != 0)
                {
                    foreach (var kv in pathTasks)
                    {
                        var entity = kv.Value;
                        var result = entity?.DeepClone();
                        if (result != null)
                        {
                            SetEntityPropertyToNull(result);

                            this.CacheEnvelope.Includes[kv.Key] = result as Entity;
                        }
                    }
                }
            }

            return aggregate;
        }
    }
}
