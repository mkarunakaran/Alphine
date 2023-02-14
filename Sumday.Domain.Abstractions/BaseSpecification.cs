using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sumday.Domain.Abstractions.ExitPorts;

namespace Sumday.Domain.Abstractions
{
    public abstract class BaseSpecification<TAggregate> : ISpecification<TAggregate>
         where TAggregate : AggregateRoot
     {
        private readonly List<Expression> includes;
        private readonly List<string> includeStrings;
        private readonly ExecutionResult notification = new ExecutionResult();

        protected BaseSpecification()
        {
            var type = this.GetType();
            if (!type.HasEntityKey())
            {
                throw new InvalidOperationException($"Entity {type.Name} must define a property marked with {nameof(EntityKeyAttribute)}");
            }

            this.includes = new List<Expression>();

            this.includeStrings = new List<string>();
        }

        public IExecuteErrorNotifcations ExecutionResultNotification => this.notification;

        public IExecutionResult IsSatisfied => this.notification;

        public ExecutionResult Notification => this.notification;

        public IReadOnlyList<Expression> Includes => this.includes.AsReadOnly();

        public IReadOnlyList<string> IncludeStrings => this.includeStrings.AsReadOnly();

        public virtual ISpecification<TAggregate> Include<TEntity>(Expression<Func<TAggregate, Entity>> includeExpression)
            where TEntity : Entity
        {
            this.includes.Add(includeExpression);
            return this;
        }

        public ISpecification<TAggregate> ThenInclude<TEntity>(Func<IncludeAggregator<TEntity>, IIncludeEntityQuery<TEntity>> includeGenerator)
            where TEntity : Entity
        {
            var includeQuery = includeGenerator(new IncludeAggregator<TEntity>());
            this.includes.AddRange(includeQuery.Paths);
            return this;
        }

        public virtual ISpecification<TAggregate> Include<TEntity>(string includeExpression)
             where TEntity : Entity
        {
            this.includeStrings.Add(includeExpression);
            return this;
        }
    }
}
