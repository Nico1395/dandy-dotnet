using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Core.Encoding.Configuration;

/// <summary>
/// Provides dependency injection registration extensions for payload encoding.
/// </summary>
public static class EncodingServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures a payload encoder.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="builderAction">An action that configures encoding.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDandyRabbitMQEncoding(this IServiceCollection services, Action<EncodingConfigurationBuilder> builderAction)
    {
        var builder = new EncodingConfigurationBuilder();
        builderAction.Invoke(builder);
        var configuration = builder.Build();

        return services.AddDandyRabbitMQEncoding(configuration);
    }

    /// <summary>
    /// Adds a payload encoder from an existing <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="configuration">The encoding configuration.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the configured encoder type is invalid.</exception>
    public static IServiceCollection AddDandyRabbitMQEncoding(this IServiceCollection services, EncodingConfiguration configuration)
    {
        if (services.BuildServiceProvider().GetService(typeof(IPayloadEncoder)) != null)
            return services;

        if (configuration.EncoderImplementationType.IsAbstract)
            throw new InvalidOperationException("Encoder implementation type is abstract.");

        var payloadEncoderInterface = typeof(IPayloadEncoder);
        if (!configuration.EncoderImplementationType.IsAssignableTo(payloadEncoderInterface))
            throw new InvalidOperationException($"Encoder implementation type does not implement {payloadEncoderInterface}.");

        return services.AddSingleton(payloadEncoderInterface, configuration.EncoderImplementationType);
    }
}
