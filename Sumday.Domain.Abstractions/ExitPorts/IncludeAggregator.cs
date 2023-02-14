using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public class IncludeAggregator<TPreviousEntity>
        where TPreviousEntity : Entity
    {
        public IncludeNewEntityQuery<TPreviousEntity, TNewEntity> Include<TNewEntity>(Expression<Func<TPreviousEntity, TNewEntity>> selector)
            where TNewEntity : Entity
        {
            var id = Guid.NewGuid();
            var pathMap = new Dictionary<Guid, Expression>() { { id, selector } };

            return new IncludeNewEntityQuery<TPreviousEntity, TNewEntity>(id, pathMap);
        }
    }
}
