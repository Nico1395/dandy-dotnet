namespace DandyDotnet.Encoding.Configuration;

/// <summary>
///     Builds payload encoding configuration using a fluent API.
/// </summary>
/// <remarks>
///     <para>
///         This class provides a fluent interface for configuring an <see cref="IEncoder"/> implementation.
///         Use the <see cref="UseEncoder(Type)"/> method to specify the encoder type and
///         <see cref="UseServiceKey(object?)"/> to specify an optional service key for keyed service registration.
///     </para>
///     <para>
///         After configuring the desired options, call <see cref="Build()"/> to create the
///         <see cref="EncodingConfiguration"/> instance.
///     </para>
/// </remarks>
public sealed class EncodingConfigurationBuilder
{
    private readonly EncodingConfiguration _configuration = new();

    /// <summary>
    ///     Associates a service key with the encoding configuration for keyed service registration.
    /// </summary>
    /// <param name="serviceKey">
    ///     The service key to associate with the configuration. This key will be used to register the encoder
    ///     as a keyed service in the dependency injection container.
    /// </param>
    /// <returns>The same <see cref="EncodingConfigurationBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         When a non-null <paramref name="serviceKey"/> is provided, the encoder will be registered as a keyed service
    ///         and must be resolved using the same key value. When <see langword="null"/>, the encoder will be
    ///         registered as the default implementation of <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/>.
    ///     </para>
    /// </remarks>
    public EncodingConfigurationBuilder UseServiceKey(object? serviceKey)
    {
        _configuration.ServiceKey = serviceKey;
        return this;
    }

    /// <summary>
    ///     Selects the payload encoder implementation type.
    /// </summary>
    /// <param name="implementationType">
    ///     The <see cref="Type"/> of the class that implements <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/>
    ///     to use for encoding and decoding operations.
    /// </param>
    /// <returns>The same <see cref="EncodingConfigurationBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="implementationType"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         The specified type must implement <see cref="DandyDotnet.Encoding.Abstractions.IEncoder"/> and must
    ///         not be an abstract class. This validation is performed when the configuration is built or when
    ///         the service is registered with the dependency injection container.
    ///     </para>
    /// </remarks>
    public EncodingConfigurationBuilder UseEncoder(Type implementationType)
    {
        _configuration.EncoderType = implementationType;
        return this;
    }

    /// <summary>
    ///     Builds the encoding configuration from the current builder state.
    /// </summary>
    /// <returns>
    ///     A new <see cref="EncodingConfiguration"/> instance containing the configured settings.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This method creates a new <see cref="EncodingConfiguration"/> instance with the current settings
    ///         from the builder. The builder itself is not modified and can be reused to build additional
    ///         configurations with different settings.
    ///     </para>
    /// </remarks>
    public EncodingConfiguration Build()
    {
        return _configuration;
    }
}
