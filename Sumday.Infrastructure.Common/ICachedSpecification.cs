using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common
{
    public interface ICachedSpecification
    {
        bool ByPassCache { set; }

        object CacheKey => string.Empty;

        bool IsSlidingExpiration => false;

        TimeSpan CacheDuration => TimeSpan.FromSeconds(1);
    }
}
