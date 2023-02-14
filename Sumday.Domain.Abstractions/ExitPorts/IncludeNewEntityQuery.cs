using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public class IncludeNewEntityQuery<TNewEntity, TPreviousEntity> : IncludeEntityQuery<TNewEntity>, IIncludeNewEntityQuery<TNewEntity, TPreviousEntity>
         where TNewEntity : Entity
         where TPreviousEntity : Entity
    {
        public IncludeNewEntityQuery(Guid id, Dictionary<Guid, Expression> pathMap)
            : base(id, pathMap)
        {
        }
    }
}
