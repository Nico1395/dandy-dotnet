namespace DandyRabbitMQ.Core.Encoding.Configuration;

/// <summary>
/// Builds payload encoding configuration.
/// </summary>
public sealed class EncodingConfigurationBuilder
{
    private readonly EncodingConfiguration _configuration = new();
    /// <summary>
    /// Selects the payload encoder implementation.
    /// </summary>
    /// <param name="implementationType">The encoder implementation type.</param>
    /// <returns>This builder.</returns>
    public EncodingConfigurationBuilder UseEncoder(Type implementationType)
    {
        _configuration.EncoderImplementationType = implementationType;
        return this;
    }

    /// <summary>
    /// Builds the encoding configuration.
    /// </summary>
    /// <returns>The encoding configuration.</returns>
    public EncodingConfiguration Build()
    {
        return _configuration;
    }
}
