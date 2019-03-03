using Microsoft.Extensions.Caching.Memory;
using Ocelot.Cache;
using Ocelot.Configuration.File;
using System;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigCache : IOcelotCache<FileConfiguration> {
        private IMemoryCache _cache;
        public OcelotConfigCache(IMemoryCache memoryCache) {
            _cache = memoryCache;
        }
        public void Add(string key, FileConfiguration value, TimeSpan ttl, string region) {
            _cache.Set(key, value, ttl);
        }

        public void AddAndDelete(string key, FileConfiguration value, TimeSpan ttl, string region) {
            _cache.Remove(key);
            _cache.Set(key, value, ttl);
        }

        public void ClearRegion(string region) {
        }

        public FileConfiguration Get(string key, string region) {
            FileConfiguration cacheEntry = null;
            if (_cache.TryGetValue(key, out cacheEntry))
                return cacheEntry;
            return null;
        }
    }
}
