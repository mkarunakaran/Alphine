using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common.Http
{
    public interface IHttpService
    {
        Task<TResponse> Send<TBody, TResponse>(TBody payload, CancellationToken cancellationToken);
    }
}
