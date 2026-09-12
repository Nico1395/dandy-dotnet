using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Core.Declarations.Configuration;

/// <summary>
/// Provides dependency injection registration extensions for declarations.
/// </summary>
public static class DeclarationsServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures declaration <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="builderAction">An action that configures declarations.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDandyRabbitMQDeclarations(this IServiceCollection services, Action<DeclarationsConfigurationBuilder> builderAction)
    {
        var builder = new DeclarationsConfigurationBuilder();
        builderAction.Invoke(builder);
        var configuration = builder.Build();

        return services;
    }

    /// <summary>
    /// Adds declaration <paramref name="services"/> from an existing <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="configuration">The declaration configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDandyRabbitMQDeclarations(this IServiceCollection services, DeclarationsConfiguration configuration)
    {
        if (services.BuildServiceProvider().GetService(typeof(IDeclarer)) != null)
            return services;

        services.AddSingleton(configuration);
        services.AddSingleton<IDeclarer, Declarer>();

        return services;
    }
}
