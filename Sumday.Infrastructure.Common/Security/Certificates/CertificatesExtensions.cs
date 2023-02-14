using Microsoft.Extensions.Configuration;
using Sumday.Infrastructure.Common.Security;
using Sumday.Infrastructure.Common.Security.Certificates;
using Sumday.Infrastructure.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CertificatesExtensions
    {
        public static IServiceCollection AddCertificates(this IServiceCollection services)
        {
            services.AddSingleton<ICertificateStore, SecretsCertificateStore>();
            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var key = config.Get<string>("keyInfo:key");
                var iv = config.Get<string>("keyInfo:iv");
                return new EncryptionService(new KeyInfo(key, iv));
            });
            return services;
        }
    }
}