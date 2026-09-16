using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.StaticEndpoints;

/// <summary>
///     Extension methods for adding static endpoint mapping services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds static endpoint mapping services to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method registers the <see cref="StaticEndpointsConfiguration" /> and <see cref="IStaticEndpointMapper" />
    ///         as singleton services. The configuration is built using the provided <paramref name="builderAction" />.
    ///     </para>
    ///     <para>
    ///         After calling this method, use <see cref="WebApplicationExtensions.MapStaticEndpoints" /> on the
    ///         <see cref="WebApplication" /> to map the discovered static endpoints.
    ///     </para>
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <param name="builderAction">
    ///     An optional action to configure the <see cref="StaticEndpointsConfigurationBuilder" />.
    ///     Use this to specify which assemblies to scan for static endpoint methods.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddStaticEndpoints(this IServiceCollection services, Action<StaticEndpointsConfigurationBuilder>? builderAction = null)
    {
        var builder = new StaticEndpointsConfigurationBuilder();
        builderAction?.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddSingleton<IStaticEndpointMapper, StaticEndpointMapper>();

        return services;
    }
}