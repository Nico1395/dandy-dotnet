namespace DandyDotnet.DependencyInjection.Cache;

/// <summary>
///     Extension methods for resolving services from a <see cref="ServiceCache" />.
/// </summary>
/// <remarks>
///     <para>
///         These methods create <see cref="ServiceCacheKey" /> values from type and optional key arguments before resolving
///         services from the cache.
///     </para>
/// </remarks>
public static class ServiceCacheExtensions
{
    /// <summary>
    ///     Gets a keyed service of the specified type from the cache.
    /// </summary>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <param name="serviceKey">The key associated with the service registration.</param>
    /// <returns>The resolved service, or <see langword="null" /> if the service could not be resolved.</returns>
    public static object? Get(this ServiceCache cache, Type serviceType, object? serviceKey)
    {
        return cache.Get(new ServiceCacheKey(serviceType, serviceKey));
    }

    /// <summary>
    ///     Gets a required keyed service of the specified type from the cache.
    /// </summary>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <param name="serviceKey">The key associated with the service registration.</param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">No service of type <paramref name="serviceType" /> could be resolved.</exception>
    public static object GetRequired(this ServiceCache cache, Type serviceType, object? serviceKey)
    {
        return cache.Get(new ServiceCacheKey(serviceType, serviceKey)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }

    /// <summary>
    ///     Gets a non-keyed service of the specified type from the cache.
    /// </summary>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <returns>The resolved service, or <see langword="null" /> if the service could not be resolved.</returns>
    public static object? Get(this ServiceCache cache, Type serviceType)
    {
        return cache.Get(new ServiceCacheKey(serviceType, null));
    }

    /// <summary>
    ///     Gets a required non-keyed service of the specified type from the cache.
    /// </summary>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">No service of type <paramref name="serviceType" /> could be resolved.</exception>
    public static object GetRequired(this ServiceCache cache, Type serviceType)
    {
        return cache.Get(new ServiceCacheKey(serviceType, null)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }

    /// <summary>
    ///     Gets a keyed service of type <typeparamref name="TService" /> from the cache.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <param name="serviceKey">The key associated with the service registration.</param>
    /// <returns>The resolved service, or <see langword="null" /> if the service could not be resolved.</returns>
    public static TService? Get<TService>(this ServiceCache cache, object? serviceKey)
    {
        return (TService?)cache.Get(new ServiceCacheKey(typeof(TService), serviceKey));
    }

    /// <summary>
    ///     Gets a required keyed service of type <typeparamref name="TService" /> from the cache.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <param name="serviceKey">The key associated with the service registration.</param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">No service of type <typeparamref name="TService" /> could be resolved.</exception>
    public static TService GetRequired<TService>(this ServiceCache cache, object? serviceKey)
    {
        var serviceType = typeof(TService);
        return (TService?)cache.Get(new ServiceCacheKey(serviceType, serviceKey)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }

    /// <summary>
    ///     Gets a non-keyed service of type <typeparamref name="TService" /> from the cache.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <returns>The resolved service, or <see langword="null" /> if the service could not be resolved.</returns>
    public static TService? Get<TService>(this ServiceCache cache)
    {
        return (TService?)cache.Get(new ServiceCacheKey(typeof(TService), null));
    }

    /// <summary>
    ///     Gets a required non-keyed service of type <typeparamref name="TService" /> from the cache.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="cache">The service cache to resolve the service from.</param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">No service of type <typeparamref name="TService" /> could be resolved.</exception>
    public static TService GetRequired<TService>(this ServiceCache cache)
    {
        var serviceType = typeof(TService);
        return (TService?)cache.Get(new ServiceCacheKey(serviceType, null)) ?? throw new InvalidOperationException($"Could not resolve service of type '{serviceType.FullName}'.");
    }
}
