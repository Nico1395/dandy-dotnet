using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.Serialization;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;

/// <summary>
/// Stores the configuration used to register producer services.
/// </summary>
public sealed class ProducerConfiguration
{
    internal ConnectivityConfigurationBuilder ConnectivityConfigurationBuilder { get; set; } = new();
    internal MessagesConfigurationBuilder MessagesConfigurationBuilder { get; set; } = new();
    internal DeclarationsConfigurationBuilder DeclarationsConfiguration { get; set; } = new();

    public SerializerConfiguration? SerializerConfiguration { get; set; }
    public EncodingConfiguration? EncodingConfiguration { get; set; }
}
