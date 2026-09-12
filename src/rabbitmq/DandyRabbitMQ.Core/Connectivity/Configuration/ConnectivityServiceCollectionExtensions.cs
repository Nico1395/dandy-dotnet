using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Core.Connectivity.Configuration;

/// <summary>
/// Provides dependency injection registration extensions for connectivity.
/// </summary>
public static class ConnectivityServiceCollectionExtensions
{
    /// <summary>
    /// Adds DandyRabbitMQ connectivity <paramref name="services"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If the <see cref="IConnectionProvider"/> is already registered, the method acts idempotent and does nothing.
    ///     </para>
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which RabbitMQ connectivity services are added.</param>
    /// <param name="connectionAction">An action used to configure the RabbitMQ connection using the <see cref="ConnectivityConfigurationBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the RabbitMQ connectivity <paramref name="services"/> added.</returns>
    public static IServiceCollection AddDandyRabbitMQConnectivity(this IServiceCollection services, Action<ConnectivityConfigurationBuilder> connectionAction)
    {
        var builder = new ConnectivityConfigurationBuilder();
        connectionAction.Invoke(builder);
        var configuration = builder.Build();

        return services.AddDandyRabbitMQConnectivity(configuration);
    }

    /// <summary>
    /// Adds DandyRabbitMQ connectivity <paramref name="services"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If the <see cref="IConnectionProvider"/> is already registered, the method acts idempotent and does nothing.
    ///     </para>
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which RabbitMQ connectivity services are added.</param>
    /// <param name="configuration">A preconfigured <see cref="ConnectivityConfiguration"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the RabbitMQ connectivity <paramref name="services"/> added.</returns>
    public static IServiceCollection AddDandyRabbitMQConnectivity(this IServiceCollection services, ConnectivityConfiguration configuration)
    {
        if (services.BuildServiceProvider().GetService(typeof(IConnectionProvider)) != null)
            return services;

        services.AddSingleton(configuration);
        services.AddSingleton<IConnectionProvider, ConnectionProvider>();

        return services;
    }
}
