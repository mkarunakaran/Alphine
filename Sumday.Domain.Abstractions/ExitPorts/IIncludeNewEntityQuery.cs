using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IIncludeNewEntityQuery<TNewEntity, out TPreviousEntity> : IIncludeEntityQuery<TNewEntity>
          where TNewEntity : Entity
          where TPreviousEntity : Entity
    {
    }
}
