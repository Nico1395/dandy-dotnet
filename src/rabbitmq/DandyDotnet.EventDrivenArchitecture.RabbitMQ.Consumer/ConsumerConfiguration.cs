using System.Reflection;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;

/// <summary>
/// Stores configuration for consumer services.
/// </summary>
public sealed class ConsumerConfiguration
{
    internal ConnectivityConfigurationBuilder ConnectivityConfigurationBuilder { get; set; } = new();
    internal MessagesConfigurationBuilder MessagesConfigurationBuilder { get; set; } = new();
    internal DeclarationsConfigurationBuilder DeclarationsConfigurationBuilder { get; set; } = new();

    /// <summary>
    /// Gets the assemblies scanned for consumer implementations and messages.
    /// </summary>
    public Assembly[]? Assemblies { get; internal set; }

    /// <summary>
    /// Gets the configured consumer interceptor type.
    /// </summary>
    public Type? ConsumerInterceptorType { get; internal set; }

    /// <summary>
    /// Gets the handler for worker initialization exceptions.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnExceptionWhenInitializingWorker { get; internal set; }

    /// <summary>
    /// Gets the handler for message receiving exceptions.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnExceptionWhenReceivingMessage { get; internal set; }

    /// <summary>
    /// Gets the handler for acknowledgement exceptions.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnExceptionWhenAckOrNack { get; internal set; }

    /// <summary>
    /// Gets the handler for interceptor exceptions.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnExceptionWhenIntercepting { get; internal set; }
}
