using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Configuration;

/// <summary>
///     Represents the configuration for payload encoding.
/// </summary>
/// <remarks>
///     <para>
///         This class holds the configuration settings for creating and registering an <see cref="IEncoder"/>
///         implementation with the dependency injection container. It specifies which encoder type to use
///         and an optional service key for keyed service registration.
///     </para>
/// </remarks>
public sealed class EncodingConfiguration
{
    /// <summary>
    ///     Gets or sets the service key for keyed service registration.
    /// </summary>
    /// <value>
    ///     The service key object, or <see langword="null"/> if the encoder should be registered as the default
    ///     implementation of <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/>. The service key is used to
    ///     distinguish between multiple encoder registrations.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         When <see langword="null"/>, the encoder will be registered as the default implementation and can be
    ///         resolved by requesting an <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/> without a key.
    ///         When a non-null value is provided, the encoder must be resolved using the same key value.
    ///     </para>
    /// </remarks>
    public object? ServiceKey { get; set; }

    /// <summary>
    ///     Gets or sets the type of the encoder implementation.
    /// </summary>
    /// <value>
    ///     The <see cref="Type"/> of the class that implements <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/>
    ///     and will be instantiated to perform encoding and decoding operations.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         The type must implement <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/> and must not be
    ///         an abstract class. The default value is <see cref="Utf8Encoder"/>, which provides UTF-8 encoding.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown when setting a type that does not implement <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/>
    ///     or is an abstract class.
    /// </exception>
    public Type EncoderType { get; set; } = typeof(Utf8Encoder);
}
