using RabbitMQ.Client;

namespace DandyRabbitMQ.Core.Connectivity;

/// <summary>
/// Provides the connection to the RabbitMQ broker or cluster.
/// </summary>
/// <remarks>
///     <para>
///         This client opens a TCP connection to the broker and keeps it open for the duration of the application's runtime.
///         Thus, the provider is registered as a singleton service, and the default RabbitMQ <see cref="IConnection"/> is
///         cached and reused for every operation.
///     </para>
///     <para>
///         Implements <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> to dispose of the established connection.
///         Do not manually dispose of the provider unless you know what you are doing.
///     </para>
/// </remarks>
public interface IConnectionProvider : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets the opened connection to the RabbitMQ broker or cluster, or establishes one.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel establishing a connection. Has no effect if the connection is already established.</param>
    /// <returns>An opened <see cref="IConnection"/> that is ready to use.</returns>
    Task<IConnection> GetAsync(CancellationToken cancellationToken);
}
