using System.Collections.Concurrent;
using DandyDotnet.DependencyInjection.Abstractions;

namespace DandyDotnet.DependencyInjection.Cache;

public sealed class ServiceCache : IDisposable, IAsyncDisposable
{
    private readonly ConcurrentDictionary<ServiceCacheKey, object?> _services = [];

    public ServiceCache(
        IServiceProvider serviceProvider,
        IEnumerable<ServiceCacheKey>? injected)
    {
        ServiceProvider = serviceProvider;

        if (injected == null)
            return;

        foreach (var inject in injected)
            _services[inject] = ServiceProvider.GetRequiredKeyedOrDefaultService(inject.ServiceType, inject.ServiceKey);
    }

    public IServiceProvider ServiceProvider { get; }

    public void Dispose()
    {
        foreach (var disposable in _services.Values.OfType<IDisposable>())
            disposable.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _services.Values.OfType<IAsyncDisposable>())
            await disposable.DisposeAsync();
    }

    public object? Get(ServiceCacheKey key)
    {
        return _services.GetOrAdd(key, cacheKey => ServiceProvider.GetKeyedOrDefaultService(cacheKey.ServiceType, cacheKey.ServiceKey));
    }
}