namespace DandyDotnet.DependencyInjection.Cache;

/// <summary>
///     Extension methods for creating <see cref="ServiceCache" /> instances from an <see cref="IServiceProvider" />.
/// </summary>
public static class ServiceCacheServiceProviderExtensions
{
    /// <summary>
    ///     Creates a service cache from the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider used by the cache to resolve services.</param>
    /// <param name="builderAction">An action used to configure services that should be injected eagerly.</param>
    /// <returns>A configured <see cref="ServiceCache" /> instance.</returns>
    /// <exception cref="InvalidOperationException">
    ///     A configured injected service could not be resolved from the service provider.
    /// </exception>
    public static ServiceCache CreateServiceCache(this IServiceProvider serviceProvider, Action<ServiceCacheBuilder> builderAction)
    {
        var builder = ServiceCacheBuilder.Create(serviceProvider);
        builderAction(builder);
        return builder.Build();
    }
}
