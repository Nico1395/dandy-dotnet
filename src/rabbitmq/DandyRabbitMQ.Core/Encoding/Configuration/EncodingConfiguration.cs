using DandyRabbitMQ.Core.Encoding.Encodings;

namespace DandyRabbitMQ.Core.Encoding.Configuration;

/// <summary>
/// Configures the payload encoding implementation.
/// </summary>
public sealed class EncodingConfiguration
{
    /// <summary>
    /// Gets or sets the payload encoder implementation type.
    /// </summary>
    public Type EncoderImplementationType { get; set; } = typeof(Utf8PayloadEncoder);
}
