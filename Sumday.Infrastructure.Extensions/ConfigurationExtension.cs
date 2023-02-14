using System;
using Microsoft.Extensions.Configuration;

namespace Sumday.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static void BindTo<T>(this IConfiguration configuration, string key, T instance)
            where T : class
        {
            var configValueProperty = configuration.GetConfigValueProperty<T>(key);

            if (configValueProperty != null)
            {
                configValueProperty.CopyObject(instance, true);
            }
            else
            {
                configuration.Bind(key, instance);
            }
        }

        public static T Get<T>(this IConfiguration configuration, string key)
        {
            var configValueProperty = configuration.GetConfigValueProperty<T>(key);

            if (configValueProperty == null)
            {
                configValueProperty = configuration.GetValue<T>(key);
            }

            return configValueProperty;
        }

        public static string GetHost(this IConfiguration configuration)
        {
            var hostName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            if (hostName != null && hostName.ToLower().Contains("staging"))
            {
                return $"https://{hostName}";
            }

            var baseUrl = configuration.Get<string>("HostUrl");
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = baseUrl.TrimEnd('/');
                return baseUrl;
            }

            return "localhost";
        }

        private static T GetConfigValueProperty<T>(this IConfiguration configuration, string key)
        {
            T configValueProperty = default;
            var keys = key.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var configValue = configuration.GetSection(keys[0]).Value;
            if (configValue != null)
            {
                if (configValue.TryParse(out var configObject))
                {
                    if (keys.Length > 1)
                    {
                        if (configObject[keys[1]] != null)
                        {
                            configValueProperty = configObject[keys[1]].ToObject<T>();
                        }
                    }
                    else
                    {
                        configValueProperty = configObject.ToObject<T>();
                    }
                }
            }

            return configValueProperty;
        }
    }
}
