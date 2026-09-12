using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages.Configuration;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Configuration;

/// <summary>
/// Stores the configuration used to register producer services.
/// </summary>
public sealed class ProducerConfiguration
{
    internal ConnectivityConfigurationBuilder ConnectivityConfigurationBuilder { get; set; } = new();
    internal MessagesConfigurationBuilder MessagesConfigurationBuilder { get; set; } = new();
    internal EncodingConfigurationBuilder EncodingConfigurationBuilder { get; set; } = new();
    internal DeclarationsConfigurationBuilder DeclarationsConfiguration { get; set; } = new();
}
