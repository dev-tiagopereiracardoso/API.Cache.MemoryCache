using API.Cache.MemoryCache.Domain.Implementation.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Reflection;

namespace API.Cache.MemoryCache.Domain.Implementation.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly ILogger<MemoryService> _logger;

        private readonly IMemoryCache _memoryCache;

        private int _minuteExpirationTime;

        public MemoryService(
                ILogger<MemoryService> logger,
                IMemoryCache memoryCache,
                IConfiguration configuration
            )
        {
            _logger = logger;
            _memoryCache = memoryCache;

            SetProperty(configuration["MinuteExpirationTime"]!);
        }

        private void SetProperty(string MinuteExpirationTime)
        {
            _minuteExpirationTime = Convert.ToInt32(MinuteExpirationTime);
        }

        public string Get(string key)
        {
            try
            {
                _memoryCache.TryGetValue(key, out string? cachedItem);
                return cachedItem!;
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Erro workflow api Get (MemoryService): {Ex.Message}");
            }
            return "";
        }

        public bool Set(string key, string value)
        {
            try
            {
                var expirationTime = DateTimeOffset.Now.AddMinutes(_minuteExpirationTime);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(expirationTime);

                _memoryCache.Set(key, value, cacheEntryOptions);

                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Erro workflow api Set (MemoryService): {Ex.Message}");
            }
            return false;
        }

        public void Delete()
        {
            try
            {
                var cacheImplType = typeof(Microsoft.Extensions.Caching.Memory.MemoryCache);
                var entriesField = cacheImplType.GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
                var entries = (IDictionary)entriesField?.GetValue(_memoryCache)!;
                entries?.Clear();
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Erro workflow api Delete (MemoryService): {Ex.Message}");
            }
        }
    }
}