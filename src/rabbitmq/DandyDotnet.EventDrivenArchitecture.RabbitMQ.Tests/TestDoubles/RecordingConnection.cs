using System.Reflection;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;

public class RecordingConnection : DispatchProxy
{
    public Queue<IChannel> Channels { get; } = new();
    public List<CancellationToken> ChannelCreationTokens { get; } = [];

    public static (IConnectionProvider Provider, RecordingConnection Recording) Create(params IChannel[] channels)
    {
        var connection = Create<IConnection, RecordingConnection>();
        var recording = (RecordingConnection)(object)connection;
        foreach (var channel in channels)
            recording.Channels.Enqueue(channel);
        return (new ConnectionProvider(connection), recording);
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod?.Name != nameof(IConnection.CreateChannelAsync))
            throw new NotSupportedException($"Unexpected connection call: {targetMethod?.Name}");

        ChannelCreationTokens.Add((CancellationToken)args![1]!);
        return Task.FromResult(Channels.Dequeue());
    }

    private sealed class ConnectionProvider(IConnection connection) : IConnectionProvider
    {
        public Task<IConnection> GetAsync(CancellationToken cancellationToken) => Task.FromResult(connection);

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}