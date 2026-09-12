using RabbitMQ.Client;

namespace DandyRabbitMQ.Core.Connectivity.Configuration;

/// <summary>
/// Configuration for the <see cref="IConnectionProvider"/>.
/// </summary>
public sealed class ConnectivityConfiguration
{
    internal ConnectionFactory ConnectionFactoryInternal { get; set; } = new();

    internal List<AmqpTcpEndpoint> NodesInternal { get; set; } = [];
    /// <summary>
    /// The underlying <see cref="ConnectionFactory"/>.
    /// </summary>
    public IConnectionFactory ConnectionFactory => ConnectionFactoryInternal;

    /// <summary>
    /// List of the configured node's AMQP endpoints.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Only contains endpoints that are added via <see cref="ConnectivityConfigurationBuilder.ConnectToCluster"/>.
    ///     </para>
    /// </remarks>
    public IReadOnlyList<AmqpTcpEndpoint> Nodes => NodesInternal;

    /// <summary>
    /// Action to be invoked when an exception occurs when attempting to establish a connection.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnConnectionException { get; set; }
}
