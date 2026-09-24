using System.Reflection;
using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.Abstractions;

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
