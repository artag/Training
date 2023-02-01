using System.Collections.Concurrent;

namespace ShoppingCart;

public interface ICache
{
    // ttl means time to live.
    void Add(string key, object value, TimeSpan ttl);
    object? Get(string key);
}

public class Cache : ICache
{
    private readonly IDictionary<string, (DateTimeOffset, object)> _cache =
        new ConcurrentDictionary<string, (DateTimeOffset, object)>();

    public void Add(string key, object value, TimeSpan ttl) =>
        _cache[key] = (DateTimeOffset.UtcNow.Add(ttl), value);

    public object? Get(string productsResource)
    {
        if (_cache.TryGetValue(productsResource, out var value)
            && value.Item1 > DateTimeOffset.UtcNow)
            return value;

        _cache.Remove(productsResource);
        return null;
    }
}
