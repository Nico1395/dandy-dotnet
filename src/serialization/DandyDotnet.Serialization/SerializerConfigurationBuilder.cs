namespace DandyDotnet.Serialization;

/// <summary>
///     A builder for creating <see cref="SerializerConfiguration" /> instances in a fluent manner.
/// </summary>
/// <remarks>
///     <para>
///         This builder provides a fluent API for configuring serializer registration. It allows chaining method calls
///         to set the service key and serializer implementation in a readable way.
///     </para>
///     <para>
///         The builder is used internally by <see cref="ServiceCollectionExtensions.AddSerializer(IServiceCollection, Action{SerializerConfigurationBuilder})" />
///         to allow custom configuration of the serializer before registration.
///     </para>
/// </remarks>
/// <example>
///     <code>
///         services.AddSerializer(builder => builder
///             .UseServiceKey("mySerializerKey")
///             .UseSerializer(typeof(MySerializer), myConfig));
///     </code>
/// </example>
public sealed class SerializerConfigurationBuilder
{
    private readonly SerializerConfiguration _configuration = new();

    /// <summary>
    ///     Sets the service key for keyed dependency injection registration.
    /// </summary>
    /// <param name="serviceKey">
    ///     The service key to register the serializer with, or <see langword="null" /> for non-keyed registration.
    /// </param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
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
    public SerializerConfigurationBuilder UseServiceKey(object? serviceKey)
    {
        _configuration.ServiceKey = serviceKey;
        return this;
    }

    /// <summary>
    ///     Sets the serializer type and its configuration.
    /// </summary>
    /// <param name="serializerType">
    ///     The <see cref="Type" /> of the class that implements <see cref="Abstractions.ISerializer" />.
    ///     This must be a concrete type that has a constructor compatible with the provided configuration.
    /// </param>
    /// <param name="configuration">
    ///     An instance of the serializer-specific configuration class.
    ///     This object will be passed to the serializer constructor when it is instantiated.
    /// </param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serializerType" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         The <paramref name="serializerType" /> must implement <see cref="Abstractions.ISerializer" /> and have a constructor
    ///         that accepts the <paramref name="configuration" /> object.
    ///     </para>
    ///     <para>
    ///         This method sets both the <see cref="SerializerConfiguration.SerializerType" /> and
    ///         <see cref="SerializerConfiguration.Configuration" /> properties of the underlying configuration.
    ///     </para>
    /// </remarks>
    public SerializerConfigurationBuilder UseSerializer(Type serializerType, object configuration)
    {
        _configuration.SerializerType = serializerType;
        _configuration.Configuration = configuration;

        return this;
    }

    /// <summary>
    ///     Builds the <see cref="SerializerConfiguration" /> instance with the configured values.
    /// </summary>
    /// <returns>A new <see cref="SerializerConfiguration" /> instance with the configured values.</returns>
    /// <remarks>
    ///     <para>
    ///         This method returns the underlying configuration object. Once built, the configuration is immutable
    ///         and can be passed to <see cref="ServiceCollectionExtensions.AddSerializer(IServiceCollection, SerializerConfiguration)" />
    ///         for registration.
    ///     </para>
    /// </remarks>
    public SerializerConfiguration Build()
    {
        return _configuration;
    }
}