using DandyRabbitMQ.Core.Connectivity.Configuration;
using RabbitMQ.Client;

namespace DandyRabbitMQ.Core.Connectivity;

internal sealed class ConnectionProvider(
    IServiceProvider serviceProvider,
    ConnectivityConfiguration connectionConfiguration) : IConnectionProvider
{
    private IConnection? _connection;

    public async Task<IConnection> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_connection is { IsOpen: true })
                return _connection;

            var connectionFactory = connectionConfiguration.ConnectionFactory;
            return _connection = await connectionFactory.CreateConnectionAsync(connectionConfiguration.Nodes, cancellationToken);
        }
        catch (Exception ex)
        {
            connectionConfiguration.OnConnectionException?.Invoke(serviceProvider, ex);
            throw;
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _connection = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}
