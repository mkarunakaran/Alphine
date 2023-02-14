using System.Net.Http;

namespace Sumday.Infrastructure.Common.Http
{
    public interface IRequestBodySerializer
    {
        string SerializeBody<T>(T body);
    }
}
