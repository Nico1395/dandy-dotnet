using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.Encoding.Codes;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Encoding.Configuration;

/// <summary>
///     Provides extension methods for registering encoding services with an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>
///     <para>
///         These methods allow for easy registration of <see cref="IEncoder"/> implementations with the
///         dependency injection container. They support both simple registration with default settings and
///         fine-grained control over the encoder implementation and service key.
///     </para>
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers an <see cref="IEncoder"/> with the specified configuration action.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="action">
    ///     An action that configures the <see cref="EncodingConfigurationBuilder"/>. This action can call
    ///     methods like <see cref="EncodingConfigurationBuilderExtensions.UseUtf8PayloadEncoder"/> or
    ///     <see cref="EncodingConfigurationBuilder.UseEncoder(Type)"/> to select the encoder implementation.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         This method provides a convenient way to configure and register an encoder using a delegate.
    ///         The <paramref name="action"/> is invoked with a new <see cref="EncodingConfigurationBuilder"/>
    ///         instance, and the resulting configuration is used to register the encoder.
    ///     </para>
    ///     <para>
    ///         If <paramref name="action"/> is <see langword="null"/>, the default <see cref="Utf8Encoder"/> will be used.
    ///     </para>
    /// </remarks>
    public static IServiceCollection AddEncoder(this IServiceCollection services, Action<EncodingConfigurationBuilder>? action)
    {
        var builder = new EncodingConfigurationBuilder();
        action?.Invoke(builder);
        var configuration = builder.Build();

        return services.AddEncoder(configuration);
    }

    /// <summary>
    ///     Registers an <see cref="IEncoder"/> with default settings.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         This method registers the default <see cref="Utf8Encoder"/> implementation as a singleton service.
    ///         If an encoder has already been registered, this method returns the <paramref name="services"/>
    ///         without making changes.
    ///     </para>
    /// </remarks>
    public static IServiceCollection AddEncoder(this IServiceCollection services)
    {
        return services.AddEncoder(action: null);
    }

    /// <summary>
    ///     Registers an <see cref="IEncoder"/> with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">
    ///     The <see cref="EncodingConfiguration"/> containing the encoder type and optional service key.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the <see cref="EncodingConfiguration.EncoderType"/> is abstract or does not implement
    ///     <see cref="IEncoder"/>.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method validates the <paramref name="configuration"/> and registers the specified encoder type
    ///         as a singleton service. If the <see cref="EncodingConfiguration.ServiceKey"/> is non-null, the
    ///         encoder is registered as a keyed service with that key. Otherwise, it is registered as the default
    ///         implementation of <see cref="IEncoder"/>.
    ///     </para>
    ///     <para>
    ///         If an encoder has already been registered (with the same key or as the default), this method returns
    ///         the <paramref name="services"/> without making changes.
    ///     </para>
    /// </remarks>
    public static IServiceCollection AddEncoder(this IServiceCollection services, EncodingConfiguration configuration)
    {
        if (services.BuildServiceProvider().GetService(typeof(IEncoder)) != null)
            return services;

        if (configuration.EncoderType.IsAbstract)
            throw new InvalidOperationException("Encoder implementation type is abstract.");

        var payloadEncoderInterface = typeof(IEncoder);
        if (!configuration.EncoderType.IsAssignableTo(payloadEncoderInterface))
            throw new InvalidOperationException($"Encoder implementation type does not implement {payloadEncoderInterface}.");

        services.AddKeyedSingletonOrDefault(payloadEncoderInterface, configuration.ServiceKey, configuration.EncoderType);
        services.AddKeyedSingletonOrDefault(configuration.ServiceKey, configuration);

        return services;
    }
}
