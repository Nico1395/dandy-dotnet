using System.Diagnostics.CodeAnalysis;

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
///         Use the <see cref="SerializerConfigurationBuilder" /> to create instances of this class in a fluent manner.
///     </para>
/// </remarks>
public sealed class SerializerConfiguration
{
    /// <summary>
    ///     Gets or sets the optional service key for keyed dependency injection registration.
    /// </summary>
    /// <value>
    ///     The service key to register the serializer with, or <see langword="null" /> for non-keyed registration.
    ///     When <see langword="null" />, the serializer will be registered as a regular singleton service.
    ///     When not <see langword="null" />, the serializer will be registered as a keyed singleton service.
    /// </value>
    public object? ServiceKey { get; set; }

    /// <summary>
    ///     Gets or sets the type of the serializer implementation to register.
    /// </summary>
    /// <value>
    ///     The <see cref="Type" /> of the class that implements <see cref="Abstractions.ISerializer" />.
    ///     This must be a concrete type that has a constructor compatible with the provided <see cref="Configuration" />.
    /// </value>
    /// <remarks>
    ///     This type must implement <see cref="Abstractions.ISerializer" /> and have a constructor that accepts
    ///     the <see cref="Configuration" /> object.
    /// </remarks>
    public Type? SerializerType { get; set; }

    /// <summary>
    ///     Gets or sets the configuration object for the serializer.
    /// </summary>
    /// <value>
    ///     An instance of the serializer-specific configuration class (e.g., <see cref="NewtonsoftJson.NewtonsoftJsonConfiguration" />
    ///     or <see cref="SystemTextJson.SystemTextJsonConfiguration" />).
    ///     This object will be passed to the serializer constructor when it is instantiated.
    /// </value>
    /// <remarks>
    ///     The type of this object varies depending on the serializer implementation being configured.
    ///     Each serializer implementation library provides its own configuration class.
    /// </remarks>
    public object? Configuration { get; set; }

    /// <summary>
    ///     Determines if the current configuration is valid and can be used to register a serializer.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if both <see cref="SerializerType" /> and <see cref="Configuration" /> are not <see langword="null" />;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     A valid configuration has both the serializer type and its configuration object specified.
    ///     Without both values, the serializer cannot be properly registered.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(SerializerType))]
    [MemberNotNullWhen(true, nameof(Configuration))]
    public bool IsValid()
    {
        return SerializerType != null && Configuration != null;
    }
}