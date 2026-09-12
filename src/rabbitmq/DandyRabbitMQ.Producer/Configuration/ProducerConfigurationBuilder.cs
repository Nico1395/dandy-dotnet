using DandyRabbitMQ.Core.Connectivity.Configuration;
using DandyRabbitMQ.Core.Declarations.Configuration;
using DandyRabbitMQ.Core.Encoding.Configuration;
using DandyRabbitMQ.Core.Messages.Configuration;

namespace DandyRabbitMQ.Producer.Configuration;

/// <summary>
/// Builds the configuration for DandyRabbitMQ producer services.
/// </summary>
public sealed class ProducerConfigurationBuilder
{
    private readonly ProducerConfiguration _configuration = new();

    /// <summary>
    /// Gets or sets the RabbitMQ connectivity configuration.
    /// </summary>
    public ConnectivityConfigurationBuilder Connectivity { get; set; } = new();

    /// <summary>
    /// Gets or sets the message metadata configuration.
    /// </summary>
    public MessagesConfigurationBuilder Messages { get; set; } = new();

    /// <summary>
    /// Gets or sets the payload encoding configuration.
    /// </summary>
    public EncodingConfigurationBuilder Encoding { get; set; } = new();

    /// <summary>
    /// Gets or sets the exchange, queue, and binding declaration configuration.
    /// </summary>
    public DeclarationsConfigurationBuilder Declarations { get; set; } = new();

    internal ProducerConfiguration Build()
    {
        _configuration.ConnectivityConfigurationBuilder = Connectivity;
        _configuration.MessagesConfigurationBuilder = Messages;
        _configuration.EncodingConfigurationBuilder = Encoding;
        _configuration.DeclarationsConfiguration = Declarations;

        return _configuration;
    }
}
