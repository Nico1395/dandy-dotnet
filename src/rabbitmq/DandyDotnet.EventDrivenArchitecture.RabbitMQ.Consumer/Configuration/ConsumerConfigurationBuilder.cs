using System.Reflection;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages.Configuration;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Configuration;

/// <summary>
/// Builds consumer service configuration.
/// </summary>
public sealed class ConsumerConfigurationBuilder
{
    private readonly ConsumerConfiguration _configuration = new();

    /// <summary>
    /// Gets or sets RabbitMQ connectivity configuration.
    /// </summary>
    public ConnectivityConfigurationBuilder Connectivity { get; set; } = new();

    /// <summary>
    /// Gets or sets message metadata configuration.
    /// </summary>
    public MessagesConfigurationBuilder Messages { get; set; } = new();

    /// <summary>
    /// Gets or sets declaration configuration.
    /// </summary>
    public DeclarationsConfigurationBuilder Declarations { get; set; } = new();

    /// <summary>
    /// Sets the consumer interceptor type.
    /// </summary>
    /// <param name="interceptorType">The interceptor implementation type.</param>
    /// <returns>This builder.</returns>
    public ConsumerConfigurationBuilder UseConsumerInterceptor(Type interceptorType)
    {
        _configuration.ConsumerInterceptorType = interceptorType;
        return this;
    }

    /// <summary>
    /// Sets <paramref name="assemblies"/> scanned for consumers and messages.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>This builder.</returns>
    public ConsumerConfigurationBuilder ScanInAssemblies(params Assembly[] assemblies)
    {
        Messages.ScanInAssemblies(_configuration.Assemblies = assemblies);
        return this;
    }

    /// <summary>
    /// Sets the worker initialization exception <paramref name="handler"/>.
    /// </summary>
    /// <param name="handler">The exception handler.</param>
    /// <returns>This builder.</returns>
    public ConsumerConfigurationBuilder OnExceptionWhenInitializingWorker(Action<IServiceProvider, Exception> handler)
    {
        _configuration.OnExceptionWhenInitializingWorker = handler;
        return this;
    }

    /// <summary>
    /// Sets the message receiving exception <paramref name="handler"/>.
    /// </summary>
    /// <param name="handler">The exception handler.</param>
    /// <returns>This builder.</returns>
    public ConsumerConfigurationBuilder OnExceptionWhenReceivingMessage(Action<IServiceProvider, Exception> handler)
    {
        _configuration.OnExceptionWhenReceivingMessage = handler;
        return this;
    }

    /// <summary>
    /// Sets the acknowledgement exception <paramref name="handler"/>.
    /// </summary>
    /// <param name="handler">The exception handler.</param>
    /// <returns>This builder.</returns>
    public ConsumerConfigurationBuilder OnExceptionWhenAckOrNack(Action<IServiceProvider, Exception> handler)
    {
        _configuration.OnExceptionWhenAckOrNack = handler;
        return this;
    }

    /// <summary>
    /// Sets the interceptor exception <paramref name="handler"/>.
    /// </summary>
    /// <param name="handler">The exception handler.</param>
    /// <returns>This builder.</returns>
    public ConsumerConfigurationBuilder OnExceptionWhenIntercepting(Action<IServiceProvider, Exception> handler)
    {
        _configuration.OnExceptionWhenIntercepting = handler;
        return this;
    }

    internal ConsumerConfiguration Build()
    {
        _configuration.ConnectivityConfigurationBuilder = Connectivity;
        _configuration.MessagesConfigurationBuilder = Messages;
        _configuration.DeclarationsConfigurationBuilder = Declarations;

        return _configuration;
    }
}
