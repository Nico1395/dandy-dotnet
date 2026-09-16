namespace DandyDotnet.DependencyInjection.Cache;

/// <summary>
///     Extension methods for configuring services to inject into a <see cref="ServiceCacheBuilder" />.
/// </summary>
/// <remarks>
///     <para>
///         Injected services are resolved when the <see cref="ServiceCache" /> is built, rather than when they are first
///         requested from the cache.
///     </para>
/// </remarks>
public static class ServiceCacheBuilderExtensions
{
    /// <summary>
    ///     Adds a keyed service to be resolved when the cache is built.
    /// </summary>
    /// <param name="builder">The service cache builder to configure.</param>
    /// <param name="serviceType">The type of service to inject.</param>
    /// <param name="serviceKey">The key associated with the service registration.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceCacheBuilder Inject(this ServiceCacheBuilder builder, Type serviceType, object? serviceKey)
    {
        return builder.Inject(new ServiceCacheKey(serviceType, serviceKey));
    }

    /// <summary>
    ///     Adds a non-keyed service to be resolved when the cache is built.
    /// </summary>
    /// <param name="builder">The service cache builder to configure.</param>
    /// <param name="serviceType">The type of service to inject.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceCacheBuilder Inject(this ServiceCacheBuilder builder, Type serviceType)
    {
        return builder.Inject(new ServiceCacheKey(serviceType, null));
    }
    
    /// <summary>
    ///     Adds a keyed service of type <typeparamref name="TService" /> to be resolved when the cache is built.
    /// </summary>
    /// <typeparam name="TService">The type of service to inject.</typeparam>
    /// <param name="builder">The service cache builder to configure.</param>
    /// <param name="serviceKey">The key associated with the service registration.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceCacheBuilder Inject<TService>(this ServiceCacheBuilder builder, object? serviceKey)
    {
        return builder.Inject(new ServiceCacheKey(typeof(TService), serviceKey));
    }

    /// <summary>
    ///     Adds a non-keyed service of type <typeparamref name="TService" /> to be resolved when the cache is built.
    /// </summary>
    /// <typeparam name="TService">The type of service to inject.</typeparam>
    /// <param name="builder">The service cache builder to configure.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceCacheBuilder Inject<TService>(this ServiceCacheBuilder builder)
    {
        return builder.Inject(new ServiceCacheKey(typeof(TService), null));
    }
}
