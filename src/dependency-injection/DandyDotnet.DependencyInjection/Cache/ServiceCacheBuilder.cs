namespace DandyDotnet.DependencyInjection.Cache;

public sealed class ServiceCacheBuilder(IServiceProvider serviceProvider)
{
    private readonly List<ServiceCacheKey> _injected = [];

    public static ServiceCacheBuilder Create(IServiceProvider serviceProvider)
    {
        return new ServiceCacheBuilder(serviceProvider);
    }

    public ServiceCacheBuilder Inject(ServiceCacheKey cacheKey)
    {
        _injected.Add(cacheKey);
        return this;
    }

    public ServiceCacheBuilder Inject(params IEnumerable<ServiceCacheKey> cacheKeys)
    {
        _injected.AddRange(cacheKeys);
        return this;
    }

    public ServiceCache Build()
    {
        return new ServiceCache(serviceProvider, _injected.Distinct());
    }
}