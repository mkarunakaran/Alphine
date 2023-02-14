using System.Security.Cryptography.X509Certificates;

namespace Sumday.Infrastructure.Common.Security.Certificates
{
    public interface ICertificateStore
    {
        X509Certificate2 Get(string certificateName);
    }
}
