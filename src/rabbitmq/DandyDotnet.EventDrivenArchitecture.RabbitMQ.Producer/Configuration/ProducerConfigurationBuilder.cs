using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Configuration;

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
    /// Gets or sets the exchange, queue, and binding declaration configuration.
    /// </summary>
    public DeclarationsConfigurationBuilder Declarations { get; set; } = new();

    internal ProducerConfiguration Build()
    {
        _configuration.ConnectivityConfigurationBuilder = Connectivity;
        _configuration.MessagesConfigurationBuilder = Messages;
        _configuration.DeclarationsConfiguration = Declarations;

        return _configuration;
    }
}
