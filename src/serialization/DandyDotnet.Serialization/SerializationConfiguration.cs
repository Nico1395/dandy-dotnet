namespace DandyDotnet.Serialization;

/// <summary>
///     Configuration options for serializer registration in the dependency injection container.
/// </summary>
/// <remarks>
///     <para>
///         This class holds all the necessary information to configure and register a serializer implementation
///         with the dependency injection container. It includes the service key for keyed registration, the serializer type,
///         and its specific configuration.
///     </para>
///     <para>
///         Use the <see cref="SerializationConfigurationBuilder" /> to create instances of this class in a fluent manner.
///     </para>
/// </remarks>
public sealed class SerializationConfiguration
{
    /// <summary>
    ///     Gets or sets the optional service key for keyed dependency injection registration.
    /// </summary>
    /// <value>
    ///     The service key to register the serializer with, or <see langword="null" /> for non-keyed registration.
    ///     When <see langword="null" />, the serializer will be registered as a regular singleton service.
    ///     When not <see langword="null" />, the serializer will be registered as a keyed singleton service.
    /// </value>
    public object? ServiceKey { get; internal set; }

    /// <summary>
    /// Configuration of the concrete serializer implementation.
    /// </summary>
    public SerializerConfiguration? SerializerConfiguration { get; internal set; }
}