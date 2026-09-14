namespace DandyDotnet.DependencyInjection.Cache;

public static class ServiceCacheExtensions
{
    public static object? Get(this ServiceCache cache, Type serviceType, object? serviceKey)
    {
        return cache.Get(new ServiceCacheKey(serviceType, serviceKey));
    }

    public static object GetRequired(this ServiceCache cache, Type serviceType, object? serviceKey)
    {
        return cache.Get(new ServiceCacheKey(serviceType, serviceKey)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }

    public static object? Get(this ServiceCache cache, Type serviceType)
    {
        return cache.Get(new ServiceCacheKey(serviceType, null));
    }

    public static object GetRequired(this ServiceCache cache, Type serviceType)
    {
        return cache.Get(new ServiceCacheKey(serviceType, null)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }

    public static TService? Get<TService>(this ServiceCache cache, object? serviceKey)
    {
        return (TService?)cache.Get(new ServiceCacheKey(typeof(TService), serviceKey));
    }

    public static TService GetRequired<TService>(this ServiceCache cache, object? serviceKey)
    {
        var serviceType = typeof(TService);
        return (TService?)cache.Get(new ServiceCacheKey(serviceType, serviceKey)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }

    public static TService? Get<TService>(this ServiceCache cache)
    {
        return (TService?)cache.Get(new ServiceCacheKey(typeof(TService), null));
    }

    public static TService GetRequired<TService>(this ServiceCache cache)
    {
        var serviceType = typeof(TService);
        return (TService?)cache.Get(new ServiceCacheKey(serviceType, null)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }
}