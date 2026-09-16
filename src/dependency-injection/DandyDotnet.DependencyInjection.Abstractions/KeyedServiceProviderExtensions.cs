using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Abstractions;

/// <summary>
///     Extension methods for <see cref="IServiceProvider" /> that make keyed service resolution optional.
/// </summary>
/// <remarks>
///     <para>
///         These helpers route to the regular <see cref="IServiceProvider" /> APIs when
///         <c>serviceKey</c> is <see langword="null" />, and to the keyed-service APIs in
///         <see cref="ServiceProviderKeyedServiceExtensions" /> otherwise.
///     </para>
///     <para>
///         This is useful when the caller accepts an optional key and wants a single code-path that works for both
///         keyed and non-keyed registrations.
///     </para>
/// </remarks>
public static class KeyedServiceProviderExtensions
{
    /// <summary>
    ///     Gets a service of the specified type, using keyed resolution when a key is provided.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the service from.</param>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <param name="serviceKey">
    ///     The service key to use for keyed resolution, or <see langword="null" /> to use non-keyed resolution.
    /// </param>
    /// <returns>
    ///     The resolved service, or <see langword="null" /> if the service could not be resolved.
    /// </returns>
    public static object? GetKeyedOrDefaultService(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetService(serviceType)
            : serviceProvider.GetKeyedService(serviceType, serviceKey);
    }
    
    /// <summary>
    ///     Gets a service of type <typeparamref name="TService" />, using keyed resolution when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="serviceProvider">The service provider to resolve the service from.</param>
    /// <param name="serviceKey">
    ///     The service key to use for keyed resolution, or <see langword="null" /> to use non-keyed resolution.
    /// </param>
    /// <returns>
    ///     The resolved service, or <see langword="null" /> if the service could not be resolved.
    /// </returns>
    public static TService? GetKeyedOrDefaultService<TService>(this IServiceProvider serviceProvider, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetService<TService>()
            : serviceProvider.GetKeyedService<TService>(serviceKey);
    }

    /// <summary>
    ///     Gets the required service of the specified type, using keyed resolution when a key is provided.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the service from.</param>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <param name="serviceKey">
    ///     The service key to use for keyed resolution, or <see langword="null" /> to use non-keyed resolution.
    /// </param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">
    ///     No service of type <paramref name="serviceType" /> could be resolved.
    /// </exception>
    public static object GetRequiredKeyedOrDefaultService(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetRequiredService(serviceType)
            : serviceProvider.GetRequiredKeyedService(serviceType, serviceKey);
    }

    /// <summary>
    ///     Gets the required service of type <typeparamref name="TService" />, using keyed resolution when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <param name="serviceProvider">The service provider to resolve the service from.</param>
    /// <param name="serviceKey">
    ///     The service key to use for keyed resolution, or <see langword="null" /> to use non-keyed resolution.
    /// </param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">
    ///     No service of type <typeparamref name="TService" /> could be resolved.
    /// </exception>
    public static TService GetRequiredKeyedOrDefaultService<TService>(this IServiceProvider serviceProvider, object? serviceKey)
        where TService : notnull
    {
        return serviceKey == null
            ? serviceProvider.GetRequiredService<TService>()
            : serviceProvider.GetRequiredKeyedService<TService>(serviceKey);
    }

    /// <summary>
    ///     Gets all services of the specified type, using keyed resolution when a key is provided.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the services from.</param>
    /// <param name="serviceType">The type of the services to resolve.</param>
    /// <param name="serviceKey">
    ///     The service key to use for keyed resolution, or <see langword="null" /> to use non-keyed resolution.
    /// </param>
    /// <returns>An enumeration of resolved services.</returns>
    public static IEnumerable<object?> GetKeyedServices(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetServices(serviceType)
            : ServiceProviderKeyedServiceExtensions.GetKeyedServices(serviceProvider, serviceType, serviceKey);
    }

    /// <summary>
    ///     Gets all services of type <typeparamref name="TService" />, using keyed resolution when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The type of services to resolve.</typeparam>
    /// <param name="serviceProvider">The service provider to resolve the services from.</param>
    /// <param name="serviceKey">
    ///     The service key to use for keyed resolution, or <see langword="null" /> to use non-keyed resolution.
    /// </param>
    /// <returns>An enumeration of resolved services.</returns>
    public static IEnumerable<TService?> GetKeyedServices<TService>(this IServiceProvider serviceProvider, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetServices<TService>()
            : ServiceProviderKeyedServiceExtensions.GetKeyedServices<TService>(serviceProvider, serviceKey);
    }
}
