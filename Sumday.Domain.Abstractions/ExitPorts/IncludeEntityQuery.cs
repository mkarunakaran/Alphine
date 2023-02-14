using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public class IncludeEntityQuery<TEntity> : IIncludeEntityQuery<TEntity>
       where TEntity : Entity
    {
        public IncludeEntityQuery(Guid id, Dictionary<Guid, Expression> pathMap)
        {
            this.Id = id;
            this.PathMap = pathMap;
        }

        public Guid Id { get; }

        public Dictionary<Guid, Expression> PathMap { get; } = new Dictionary<Guid, Expression>();

        public IEnumerable<Expression> Paths => this.PathMap.Select(x => x.Value).ToList();
    }
}
