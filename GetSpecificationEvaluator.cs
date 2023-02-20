using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ObjectCloner.Extensions;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Surpas
{
    public class GetSpecificationEvaluator<TAggregate, TSpecification> : IGetSpecificationEvaluator<TAggregate, TSpecification>
         where TAggregate : AggregateRoot
         where TSpecification : ISpecification<TAggregate>
    {
        private readonly IServiceProvider serviceProvider;

        public GetSpecificationEvaluator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public virtual async Task<TAggregate> Get(TSpecification specification, CancellationToken cancellationToken = default)
        {
            var aggregateService = this.serviceProvider.GetRequiredService<IAggregateService<TAggregate>>();
            var aggregate = await aggregateService.Get(specification, cancellationToken);
            await this.PopulateEntities(aggregate, specification, cancellationToken);
            return aggregate;
        }

        protected static void SetEntityPropertyToNull(object obj)
        {
            var entityProps = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(t => t.PropertyType.IsSubclassOf(typeof(Entity))).ToList();

            foreach (var prop in entityProps)
            {
                prop.SetValue(obj, null);
            }
        }

        protected async Task<Dictionary<string, Entity>> PopulateEntities(TAggregate aggregate, TSpecification specification, CancellationToken cancellationToken = default)
        {
            var entites = new Dictionary<string, Entity>();
            var pathTasks = new Dictionary<string, Task<object>>();

            var memberObjects = new Dictionary<string, Type>();
            var baseSpecification = specification as BaseSpecification<TAggregate>;
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
                if (typeof(Entity).IsAssignableFrom(memeberType))
                {
                    var entityType = typeof(IEntityService<,>);
                    Type[] typeArgs = { typeof(TAggregate), memeberType };
                    Type constructed = entityType.MakeGenericType(typeArgs);
                    var entityService = this.serviceProvider.GetService(constructed);
                    if (entityService != null)
                    {
                        var method = constructed.GetMethod("GetAndSet");

                        var task = method.InvokeAsync<object>(entityService, new object[] { aggregate, specification, cancellationToken });
                        pathTasks.Add(childPath.Key, task);
                    }
                }
            }

            if (pathTasks.Count != 0)
            {
                await Task.WhenAll(pathTasks.Values.Where(tsk => tsk != null));
                foreach (var kv in pathTasks)
                {
                    if (kv.Value != null)
                    {
                        var entity = await kv.Value;
                        var result = entity?.DeepClone();

                        if (result != null)
                        {
                            SetEntityPropertyToNull(result);
                            entites[kv.Key] = result as Entity;
                        }
                    }
                }
            }

            return entites;
        }
    }
}
