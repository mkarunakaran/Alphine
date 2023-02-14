using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface ISpecification<TAggregate>
       where TAggregate : AggregateRoot
    {
        IExecutionResult IsSatisfied { get; }

        IExecuteErrorNotifcations ExecutionResultNotification { get; }

        ISpecification<TAggregate> Include<TEntity>(Expression<Func<TAggregate, Entity>> includeExpression)
             where TEntity : Entity;

        ISpecification<TAggregate> ThenInclude<TEntity>(Func<IncludeAggregator<TEntity>, IIncludeEntityQuery<TEntity>> includeGenerator)
             where TEntity : Entity;

        ISpecification<TAggregate> Include<TEntity>(string includeExpression)
           where TEntity : Entity;
    }
}
