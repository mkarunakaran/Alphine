using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sumday.Infrastructure.Common.Security;

namespace Sumday.Infrastructure.Common.Caching
{
    public class DistributedCacheProvider : ICacheProvider
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ////ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ////NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CachingDataContractResolver(),
           //// ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        private readonly IDistributedCache cache;

        private readonly ILogger logger;

        private readonly EncryptionService encryptionService;

        public DistributedCacheProvider(IDistributedCache cache, ILoggerFactory loggerFactory, EncryptionService encryptionService)
        {
            this.cache = cache;
            this.logger = loggerFactory.CreateLogger<DistributedCacheProvider>();
            this.encryptionService = encryptionService;
        }

        public async Task<T> Get<T>(string key, CancellationToken cancellationToken)
        {
            try
            {
                var buffer = await this.cache.GetAsync(key, cancellationToken);

                if (buffer == null)
                {
                    return default;
                }

                var obj = this.DeserializeAsync<T>(buffer);

                return obj;
            }
            catch (Exception exception)
            {
                this.logger.LogWarning($"Invalid cache object key: {key}", exception);
                await this.Remove(key, cancellationToken);

                return default;
            }
        }

        public Task Remove(string key, CancellationToken cancellationToken)
        {
            return this.cache.RemoveAsync(key, cancellationToken);
        }

        public async Task Set(string key, object obj, TimeSpan cacheDuration, bool isSlidingExpiration, CancellationToken cancellationToken)
        {
            var buffer = this.SerializeAsync(obj);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = cacheDuration };
            if (isSlidingExpiration)
            {
                options = new DistributedCacheEntryOptions { SlidingExpiration = cacheDuration };
            }

            await this.cache.SetAsync(key, buffer, options, cancellationToken);
        }

        private byte[] SerializeAsync<T>(T value)
        {
            var json = JsonConvert.SerializeObject(value, Settings);
            var protectedString = this.encryptionService.Protect(json);
            return Encoding.UTF8.GetBytes(protectedString);
        }

        private T DeserializeAsync<T>(byte[] buffer)
        {
            var protectedString = Encoding.UTF8.GetString(buffer);
            var json = this.encryptionService.Unprotect(protectedString);
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
