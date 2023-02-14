using System;
using Azure.Identity;

namespace Microsoft.Extensions.Configuration
{
    public static class SecretsExtensions
    {
        public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder, string configurationPath, bool isDevelopment)
        {
            var builtConfig = builder.Build();
            var secretsId = builtConfig["secrets:id"];
            if (isDevelopment)
            {
                builder.AddJsonFile($"{configurationPath}/secrets.json", true, true);
            }
            else
            {
                builder.AddAzureKeyVault(new System.Uri($"https://{secretsId}.vault.azure.net/"), new DefaultAzureCredential());
            }

            builder.AddEnvironmentVariables();  // to overwrite any settings from appservice configuration blade
            return builder;
        }
    }
}
