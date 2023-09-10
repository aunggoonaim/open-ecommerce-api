using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenCommerce.Application.Repositories;

namespace OpenCommerce.Persistence.Repositories;

public class MemCacheService : IMemCacheService
{
    private readonly IMemoryCache _cache;
    private MemoryCacheEntryOptions _entryOptions;

    public MemCacheService(IOptions<MemCacheServiceSettings> memCacheServiceSettings, IMemoryCache cache)
    {
        var _memCacheServiceSettings = memCacheServiceSettings.Value;
        _entryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(_memCacheServiceSettings.SlidingExpirationSecounds),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_memCacheServiceSettings.AbsoluteExpirationRelativeToNowSecounds)
        };
        _cache = cache;
    }

    public void Set<T>(string key, T obj) where T : new()
    {
        _cache.Set(key, obj, this._entryOptions);
    }

    public T? Get<T>(string key) where T : new()
    {
        _cache.TryGetValue(key, out var value);
        return value is null ? default(T) : (T)value;
    }
}

public class MemCacheServiceSettings
{
    public double SlidingExpirationSecounds { get; set; }
    public double AbsoluteExpirationRelativeToNowSecounds { get; set; }
}
