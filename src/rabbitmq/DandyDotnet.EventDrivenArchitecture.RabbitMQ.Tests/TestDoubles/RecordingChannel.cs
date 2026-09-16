using System.Reflection;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;

// A strict adapter for the small subset of IChannel exercised by receiver/declaration tests.
// Unexpected calls fail rather than receiving a default value that could conceal a regression.
public class RecordingChannel : DispatchProxy
{
    public List<ChannelCall> Calls { get; } = [];
    public Exception? AcknowledgementException { get; set; }
    public Exception? PublishException { get; set; }

    public static (IChannel Channel, RecordingChannel Recording) Create()
    {
        var channel = Create<IChannel, RecordingChannel>();
        return (channel, (RecordingChannel)(object)channel);
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var method = targetMethod ?? throw new ArgumentNullException(nameof(targetMethod));
        var arguments = method.GetParameters()
            .Select((parameter, index) => (parameter.Name!, Value: args![index]))
            .ToDictionary(item => item.Item1, item => item.Value);
        Calls.Add(new ChannelCall(method.Name, arguments));

        return method.Name switch
        {
            nameof(IChannel.BasicPublishAsync) => PublishException is null ? ValueTask.CompletedTask : ValueTask.FromException(PublishException),
            nameof(IChannel.BasicAckAsync) or nameof(IChannel.BasicNackAsync) =>
                AcknowledgementException is null ? ValueTask.CompletedTask : ValueTask.FromException(AcknowledgementException),
            nameof(IChannel.ExchangeDeclareAsync) or nameof(IChannel.BasicQosAsync) or nameof(IChannel.QueueBindAsync) => Task.CompletedTask,
            nameof(IChannel.QueueDeclareAsync) => Task.FromResult(new QueueDeclareOk((string)arguments["queue"]!, 0, 0)),
            _ => throw new NotSupportedException($"Unexpected channel call: {method.Name}")
        };
    }

    public sealed record ChannelCall(string Method, IReadOnlyDictionary<string, object?> Arguments);
}