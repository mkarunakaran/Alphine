using System;
using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace Sumday.Infrastructure.Common.Security.Certificates
{
    public class SecretsCertificateStore : ICertificateStore
    {
        private readonly IConfiguration configuration;

        public SecretsCertificateStore(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public X509Certificate2 Get(string certificateName)
        {
            var environment = this.configuration["ASPNETCORE_ENVIRONMENT"] ?? this.configuration["DOTNET_ENVIRONMENT"];
            if (environment == "Development")
            {
                return GetFromUserStore(certificateName);
            }

            return this.GetFromKeyValult(certificateName);
        }

        private static X509Certificate2 GetFromUserStore(string certificateName)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, certificateName, true);

            var certificate = certs.Count == 1 ? certs[0] : null;

            return certificate;
        }

        private X509Certificate2 GetFromKeyValult(string certificateName)
        {
            var secretsId = this.configuration["secrets:id"];
            var keyVaultUrl = new Uri($"https://{secretsId}.vault.azure.net/");

            var client = new CertificateClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());
            var certificateSecret = client.GetCertificate(certificateName);
            var secretClient = new SecretClient(keyVaultUrl, new DefaultAzureCredential());
            var secretId = certificateSecret.Value.SecretId;
            var segments = secretId.Segments;
            string secretName = segments[2].Trim('/');
            string version = segments[3].TrimEnd('/');
            var secret = secretClient.GetSecret(secretName, version);

            var keyVaultSecret = secret.Value;
            byte[] privateKeyBytes = Convert.FromBase64String(keyVaultSecret.Value);

            var certificate = new X509Certificate2(privateKeyBytes);

            return certificate;
        }
    }
}
