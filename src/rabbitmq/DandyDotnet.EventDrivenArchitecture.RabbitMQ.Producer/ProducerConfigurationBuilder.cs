using System.Reflection;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;

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

    /// <summary>
    /// Sets <paramref name="assemblies"/> scanned for messages.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>This builder.</returns>
    public ProducerConfigurationBuilder ScanInAssemblies(params Assembly[] assemblies)
    {
        Messages.ScanInAssemblies(assemblies);
        return this;
    }

    internal ProducerConfiguration Build()
    {
        _configuration.ConnectivityConfigurationBuilder = Connectivity;
        _configuration.MessagesConfigurationBuilder = Messages;
        _configuration.DeclarationsConfiguration = Declarations;

        return _configuration;
    }
}
