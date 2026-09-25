using DandyDotnet.Serialization.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization;

/// <summary>
///     A builder for creating <see cref="SerializationConfiguration" /> instances in a fluent manner.
/// </summary>
/// <remarks>
///     <para>
///         This builder provides a fluent API for configuring serializer registration. It allows chaining method calls
///         to set the service key and serializer implementation readably.
///     </para>
///     <para>
///         The builder is used internally by <see cref="ServiceCollectionExtensions.AddSerialization" />
///         to allow custom configuration of the serializer before registration.
///     </para>
/// </remarks>
/// <example>
///     <code>
///         services.AddSerialization(builder => builder
///             .UseServiceKey("mySerializerKey")
///             .UseSerializer(typeof(MySerializer), myConfig));
///     </code>
/// </example>
public sealed class SerializationConfigurationBuilder
{
    private readonly SerializationConfiguration _configuration = new();

    /// <summary>
    ///     Sets the service key for keyed dependency injection registration.
    /// </summary>
    /// <param name="serviceKey">
    ///     The service key to register the serializer with, or <see langword="null" /> for non-keyed registration.
    /// </param>
    /// <returns>The same <see cref="SerializationConfigurationBuilder" /> instance for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         When a service key is provided, the serializer will be registered as a keyed singleton service.
    ///         This allows multiple serializers to coexist in the container, each accessible via its own key.
    ///     </para>
    ///     <para>
    ///         When <see langword="null" /> is provided (or not set), the serializer will be registered as a regular singleton service
    ///         and will be the default serializer resolved from the container.
    ///     </para>
    /// </remarks>
    public SerializationConfigurationBuilder UseServiceKey(object? serviceKey)
    {
        _configuration.ServiceKey = serviceKey;
        return this;
    }

    /// <summary>
    /// Sets the configuration for an <see cref="ISerializer"/> implementation.
    /// </summary>
    /// <param name="configuration">Configuration of the serializer implementation.</param>
    /// <returns>The same <see cref="SerializationConfigurationBuilder" /> instance for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         Should only be used when implementing an <see cref="ISerializer"/>.
    ///     </para>
    /// </remarks>
    public SerializationConfigurationBuilder UseSerializer(SerializerConfiguration configuration)
    {
        _configuration.SerializerConfiguration = configuration;
        return this;
    }

    /// <summary>
    ///     Builds the <see cref="SerializationConfiguration" /> instance with the configured values.
    /// </summary>
    /// <returns>A new <see cref="SerializationConfiguration" /> instance with the configured values.</returns>
    /// <remarks>
    ///     <para>
    ///         This method returns the underlying configuration object. Once built, the configuration is immutable
    ///         and can be passed to <see cref="ServiceCollectionExtensions.AddSerialization(Microsoft.Extensions.DependencyInjection.IServiceCollection,DandyDotnet.Serialization.SerializationConfiguration)" />
    ///         for registration.
    ///     </para>
    /// </remarks>
    public SerializationConfiguration Build()
    {
        return _configuration;
    }
}