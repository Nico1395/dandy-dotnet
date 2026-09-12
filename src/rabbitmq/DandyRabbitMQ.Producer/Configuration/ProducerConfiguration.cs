using DandyRabbitMQ.Core.Connectivity.Configuration;
using DandyRabbitMQ.Core.Declarations.Configuration;
using DandyRabbitMQ.Core.Encoding.Configuration;
using DandyRabbitMQ.Core.Messages.Configuration;

namespace DandyRabbitMQ.Producer.Configuration;

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
