using System;
using System.Linq.Expressions;
using Sumday.Domain.Abstractions.ExitPorts;

namespace Sumday.Domain.Abstractions
{
    public abstract class BaseAllSpecification<TAggregate> : IAllSpecification<TAggregate>
         where TAggregate : AggregateRoot
    {
        private readonly ExecutionResult notification = new ExecutionResult();

        public int Take { get; protected set; }

        public int Skip { get; protected set; }

        public bool IsPagingEnabled { get; protected set; } = false;

        public Expression<Func<TAggregate, object>> OrderBy { get; protected set; }

        public Expression<Func<TAggregate, object>> OrderByDescending { get; protected set; }

        public AllSpecificationtType AllSpecificationtType { get; protected set; } = AllSpecificationtType.GetAll;

        public IExecuteErrorNotifcations ExecutionResultNotification => this.notification;

        public IExecutionResult IsSatisfied => this.notification;

        public ExecutionResult Notification => this.notification;

        public IAllSpecification<TAggregate> Paging(int skip, int take)
        {
            this.Skip = skip;
            this.Take = take;
            this.IsPagingEnabled = true;
            return this;
        }

        protected virtual void ApplyOrderBy(Expression<Func<TAggregate, object>> orderByExpression)
        {
            this.OrderBy = orderByExpression;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<TAggregate, object>> orderByDescendingExpression)
        {
            this.OrderByDescending = orderByDescendingExpression;
        }
    }
}
