namespace OpenCommerce.Application.Repositories;
public interface IMemCacheService
{
    void Set<T>(string key, T obj) where T : new();
    T? Get<T>(string key) where T : new();
}
