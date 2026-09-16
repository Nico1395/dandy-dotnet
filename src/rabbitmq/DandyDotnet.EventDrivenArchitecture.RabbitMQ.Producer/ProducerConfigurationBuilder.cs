using System.Reflection;
using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.Abstractions;
using Microsoft.Extensions.DependencyInjection;

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
    /// Gets or sets the serializer configuration.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If not set, the framework assumes the <see cref="ISerializer"/> has been added to
    ///         the <see cref="IServiceCollection"/> manually and <b>without</b> a service key.
    ///     </para>
    /// </remarks>
    public SerializerConfigurationBuilder? Serializer { get; set; }

    /// <summary>
    /// Gets or sets the encoding configuration.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If not set, the framework assumes the <see cref="IEncoder"/> has been added to
    ///         the <see cref="IServiceCollection"/> manually and <b>without</b> a service key.
    ///     </para>
    /// </remarks>
    public EncodingConfigurationBuilder? Encoder { get; set; }

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
        _configuration.SerializerConfiguration = Serializer?.Build();
        _configuration.EncodingConfiguration = Encoder?.Build();

        return _configuration;
    }
}
