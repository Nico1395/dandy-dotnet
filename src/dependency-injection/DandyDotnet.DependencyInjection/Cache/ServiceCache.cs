using System.Collections.Concurrent;
using DandyDotnet.DependencyInjection.Abstractions;

namespace DandyDotnet.DependencyInjection.Cache;

/// <summary>
///     Caches services resolved from an <see cref="IServiceProvider" /> by type and optional service key.
/// </summary>
/// <remarks>
///     <para>
///         Services configured as injected services are resolved immediately by the constructor. All other services are
///         resolved lazily by <see cref="Get(ServiceCacheKey)" /> and stored for subsequent calls using the same
///         <see cref="ServiceCacheKey" />.
///     </para>
///     <para>
///         Disposing the cache disposes cached service instances that implement <see cref="IDisposable" />. Asynchronously
///         disposing the cache disposes cached service instances that implement <see cref="IAsyncDisposable" />.
///     </para>
/// </remarks>
public sealed class ServiceCache : IDisposable, IAsyncDisposable
{
    private readonly ConcurrentDictionary<ServiceCacheKey, object?> _services = [];

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceCache" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve services.</param>
    /// <param name="injected">
    ///     Service keys to resolve immediately, or <see langword="null" /> to create an empty lazy cache.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     A configured injected service could not be resolved from the service provider.
    /// </exception>
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

    /// <summary>
    ///     Gets the service provider used by this cache to resolve services.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    ///     Disposes all cached services that implement <see cref="IDisposable" />.
    /// </summary>
    public void Dispose()
    {
        foreach (var disposable in _services.Values.OfType<IDisposable>())
            disposable.Dispose();
    }

    /// <summary>
    ///     Asynchronously disposes all cached services that implement <see cref="IAsyncDisposable" />.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _services.Values.OfType<IAsyncDisposable>())
            await disposable.DisposeAsync();
    }

    /// <summary>
    ///     Gets a service from the cache, resolving and caching it if this is the first request for the key.
    /// </summary>
    /// <param name="key">The service cache key to resolve.</param>
    /// <returns>
    ///     The resolved service, or <see langword="null" /> if the service could not be resolved.
    /// </returns>
    public object? Get(ServiceCacheKey key)
    {
        return _services.GetOrAdd(key, cacheKey => ServiceProvider.GetKeyedOrDefaultService(cacheKey.ServiceType, cacheKey.ServiceKey));
    }
}
