using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IAllSpecification<TAggregate>
       where TAggregate : AggregateRoot
    {
        int Take { get; }

        int Skip { get; }

        bool IsPagingEnabled { get; }

        AllSpecificationtType AllSpecificationtType { get; }

        Expression<Func<TAggregate, object>> OrderBy { get; }

        Expression<Func<TAggregate, object>> OrderByDescending { get; }

        IExecutionResult IsSatisfied { get; }

        IExecuteErrorNotifcations ExecutionResultNotification { get; }
    }
}
