using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;

/// <summary>
/// Stores the configuration used to register producer services.
/// </summary>
public sealed class ProducerConfiguration
{
    internal ConnectivityConfigurationBuilder ConnectivityConfigurationBuilder { get; set; } = new();
    internal MessagesConfigurationBuilder MessagesConfigurationBuilder { get; set; } = new();
    internal DeclarationsConfigurationBuilder DeclarationsConfiguration { get; set; } = new();

    /// <summary>
    /// Gets or sets a serializer configuration.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If not <see langword="null"/>, adds an <see cref="ISerializer"/> with the given configuration.
    ///     </para>
    ///     <para>
    ///         If multiple serializers are added, consider setting the <see cref="SerializationConfiguration.ServiceKey"/>.
    ///     </para>
    /// </remarks>
    public SerializationConfiguration? SerializerConfiguration { get; set; }
    
    /// <summary>
    /// Gets or sets an encoder configuration.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If not <see langword="null"/>, adds an <see cref="IEncoder"/> with the given configuration.
    ///     </para>
    ///     <para>
    ///         If multiple encoders are added, consider setting the <see cref="EncodingConfiguration.ServiceKey"/>.
    ///     </para>
    /// </remarks>
    public EncodingConfiguration? EncodingConfiguration { get; set; }
}
