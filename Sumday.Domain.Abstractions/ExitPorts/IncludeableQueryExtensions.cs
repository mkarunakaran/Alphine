using System;
using System.Linq.Expressions;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public static class IncludeableQueryExtensions
    {
        public static IIncludeNewEntityQuery<TEntity, TNewEntity> Include<TEntity, TPreviousEntity, TNewEntity>(
            this IIncludeNewEntityQuery<TEntity, TPreviousEntity> query,
            Expression<Func<TEntity, TNewEntity>> selector)
           where TEntity : Entity
           where TPreviousEntity : Entity
           where TNewEntity : Entity
        {
            var id = Guid.NewGuid();
            query.PathMap[id] = selector;

            return new IncludeNewEntityQuery<TEntity, TNewEntity>(id, query.PathMap);
        }

        public static IIncludeNewEntityQuery<TEntity, TNewEntity> ThenInclude<TEntity, TPreviousEntity, TNewEntity>(
            this IIncludeNewEntityQuery<TEntity, TPreviousEntity> query,
            Expression<Func<TPreviousEntity, TNewEntity>> selector)
            where TEntity : Entity
            where TPreviousEntity : Entity
            where TNewEntity : Entity
        {
            query.PathMap[query.Id] = selector;

            return new IncludeNewEntityQuery<TEntity, TNewEntity>(query.Id, query.PathMap);
        }

        ////public static IIncludeEntityQuery<TAggregate, TNewEntity> ThenInclude<TAggregate, TPreviousEntity, TNewEntity>(
        ////    this IIncludeEntityQuery<TAggregate, IEnumerable<TPreviousEntity>> query,
        ////    Expression<Func<TPreviousEntity, TNewEntity>> selector)
        ////   where TAggregate : AggregateRoot
        ////   where TPreviousEntity : Entity
        ////   where TNewEntity : Entity
        ////{
        ////    query.Visitor.Visit(selector);

        ////    var existingPath = query.PathMap[query.Id];
        ////    query.PathMap[query.Id] = $"{existingPath}.{query.Visitor.Path}";

        ////    return new IncludeEntityQuery<TAggregate, TNewEntity>(query.Id, query.PathMap);
        ////}
    }
}
