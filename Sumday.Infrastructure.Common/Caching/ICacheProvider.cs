using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common.Caching
{
    public interface ICacheProvider
    {
        Task<T> Get<T>(string key, CancellationToken cancellationToken);

        Task Set(string key, object obj, TimeSpan cacheDurationMinutes, bool isSlidingExpiration, CancellationToken cancellationToken);

        Task Remove(string key, CancellationToken cancellationToken);
    }
}
