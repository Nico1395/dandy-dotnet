namespace DandyDotnet.DependencyInjection.Cache;

/// <summary>
///     Builds a <see cref="ServiceCache" /> with services that should be resolved eagerly.
/// </summary>
/// <remarks>
///     <para>
///         Services added through <see cref="Inject(ServiceCacheKey)" /> or <see cref="Inject(IEnumerable{ServiceCacheKey})" />
///         are resolved when <see cref="Build" /> is called. Services that are not injected are resolved lazily by
///         <see cref="ServiceCache" /> the first time they are requested.
///     </para>
/// </remarks>
/// <param name="serviceProvider">The service provider used by the cache to resolve services.</param>
public sealed class ServiceCacheBuilder(IServiceProvider serviceProvider)
{
    private readonly List<ServiceCacheKey> _injected = [];

    /// <summary>
    ///     Creates a new <see cref="ServiceCacheBuilder" /> for the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider used by the cache to resolve services.</param>
    /// <returns>A new builder instance.</returns>
    public static ServiceCacheBuilder Create(IServiceProvider serviceProvider)
    {
        return new ServiceCacheBuilder(serviceProvider);
    }

    /// <summary>
    ///     Adds a service cache key whose service should be resolved when the cache is built.
    /// </summary>
    /// <param name="cacheKey">The service cache key to inject into the cache.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ServiceCacheBuilder Inject(ServiceCacheKey cacheKey)
    {
        _injected.Add(cacheKey);
        return this;
    }

    /// <summary>
    ///     Adds service cache keys whose services should be resolved when the cache is built.
    /// </summary>
    /// <param name="cacheKeys">The service cache keys to inject into the cache.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ServiceCacheBuilder Inject(params IEnumerable<ServiceCacheKey> cacheKeys)
    {
        _injected.AddRange(cacheKeys);
        return this;
    }

    /// <summary>
    ///     Builds the configured <see cref="ServiceCache" />.
    /// </summary>
    /// <returns>A service cache using the configured service provider and injected service keys.</returns>
    /// <exception cref="InvalidOperationException">
    ///     A configured injected service could not be resolved from the service provider.
    /// </exception>
    public ServiceCache Build()
    {
        return new ServiceCache(serviceProvider, _injected.Distinct());
    }
}
