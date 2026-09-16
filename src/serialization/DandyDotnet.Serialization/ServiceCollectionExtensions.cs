using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Serialization.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization;

/// <summary>
///     Extension methods for <see cref="IServiceCollection" /> to register serializers in the dependency injection container.
/// </summary>
/// <remarks>
///     <para>
///         These extensions provide multiple ways to register an <see cref="Abstractions.ISerializer" /> implementation
///         with the dependency injection container, supporting both keyed and non-keyed registration.
///     </para>
///     <para>
///         The registration follows a builder pattern, allowing full configuration of the serializer through
///         <see cref="SerializerConfigurationBuilder" />.
///     </para>
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers a serializer with the dependency injection container using a configuration action.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the serializer to.</param>
    /// <param name="configure">
    ///     An optional action that configures the <see cref="SerializerConfigurationBuilder" />.
    ///     If <see langword="null" />, the default configuration will be used (which may not be valid).
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the resulting configuration is not valid (i.e., no serializer type is specified).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method creates a <see cref="SerializerConfigurationBuilder" />, invokes the <paramref name="configure" /> action on it,
    ///         builds the configuration, and then registers the serializer using <see cref="AddSerializer(IServiceCollection, SerializerConfiguration)" />.
    ///     </para>
    ///     <para>
    ///         For typical usage, callers should use one of the serializer-specific extension methods (e.g.,
    ///         <see cref="NewtonsoftJson.SerializerConfigurationBuilderExtensions.UseNewtonsoftJson" /> or
    ///         <see cref="SystemTextJson.SerializerConfigurationBuilderExtensions.UseSystemTextJson" />) within the configure action.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         services.AddSerializer(builder => builder
    ///             .UseServiceKey("customKey")
    ///             .UseNewtonsoftJson(config => config.JsonSerializerSettings = new JsonSerializerSettings()));
    ///     </code>
    /// </example>
    public static IServiceCollection AddSerializer(this IServiceCollection services, Action<SerializerConfigurationBuilder>? configure)
    {
        var builder = new SerializerConfigurationBuilder();
        configure?.Invoke(builder);
        var configuration = builder.Build();

        return services.AddSerializer(configuration);
    }

    /// <summary>
    ///     Registers a serializer with the dependency injection container using default configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the serializer to.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the resulting configuration is not valid (i.e., no serializer type is specified).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method calls <see cref="AddSerializer(IServiceCollection, Action{SerializerConfigurationBuilder})" /> with a <see langword="null" />
    ///         configure action. The resulting configuration will be invalid unless a serializer type is specified
    ///         through other means.
    ///     </para>
    /// </remarks>
    public static IServiceCollection AddSerializer(this IServiceCollection services)
    {
        return services.AddSerializer(configure: null);
    }

    /// <summary>
    ///     Registers a serializer with the dependency injection container using the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the serializer to.</param>
    /// <param name="configuration">The serializer configuration to use for registration.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the <paramref name="configuration" /> is not valid (i.e., <see cref="SerializerConfiguration.IsValid" /> returns <see langword="false" />).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method registers the serializer implementation and its configuration as keyed or non-keyed singleton services
    ///         depending on the <see cref="SerializerConfiguration.ServiceKey" /> property.
    ///     </para>
    ///     <para>
    ///         If a serializer with the same key (or without a key if no key is specified) already exists in the container,
    ///         this method returns without making any changes (idempotent registration).
    ///     </para>
    ///     <para>
    ///         The serializer is registered with singleton lifetime, meaning the same instance will be returned for all
    ///         subsequent resolutions.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         var config = new SerializerConfigurationBuilder()
    ///             .UseNewtonsoftJson(new NewtonsoftJsonConfiguration())
    ///             .Build();
    ///         services.AddSerializer(config);
    ///     </code>
    /// </example>
    public static IServiceCollection AddSerializer(this IServiceCollection services, SerializerConfiguration configuration)
    {
        if (!configuration.IsValid())
            throw new InvalidOperationException("Serializer type is not specified.");

        if (services.BuildServiceProvider().GetKeyedOrDefaultService(typeof(ISerializer), configuration.ServiceKey) != null)
            return services;

        services.AddKeyedSingletonOrDefault(typeof(ISerializer), configuration.ServiceKey, configuration.SerializerType);
        services.AddKeyedSingletonOrDefault(configuration.Configuration.GetType(), configuration.ServiceKey, configuration.Configuration);

        return services;
    }
}