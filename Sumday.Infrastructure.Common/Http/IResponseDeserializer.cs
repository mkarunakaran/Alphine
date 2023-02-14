using System.Net.Http;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common.Http
{
    public interface IResponseDeserializer
    {
        Task<T> Deserialize<T>(string requestMessage, string responseMessage);
    }
}
