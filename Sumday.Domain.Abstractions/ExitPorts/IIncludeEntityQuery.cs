using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IIncludeEntityQuery<TEntity>
        where TEntity : Entity
    {
        Guid Id { get; }

        Dictionary<Guid, Expression> PathMap { get; }

        IEnumerable<Expression> Paths { get; }
    }
}
