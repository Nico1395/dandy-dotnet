using System.Collections.Concurrent;
using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres.Tests.Mocks;

public static class SubscriberRecorder
{
    public static ConcurrentQueue<string> Calls { get; } = new();
    public static ConcurrentQueue<Exception> Exceptions { get; } = new();

    public static void Clear()
    {
        while (Calls.TryDequeue(out _)) { }
        while (Exceptions.TryDequeue(out _)) { }
    }
}

[Subscriber(Mode = SubscriberMode.Inline)]
public sealed class InlineEventSubscriber : ISubscriber<InlineEvent>
{
    public Task HandleAsync(InlineEvent subscribed, SubscriberContext context, CancellationToken cancellationToken)
    {
        SubscriberRecorder.Calls.Enqueue($"inline:{subscribed.Id}:{context.Mode}");
        return Task.CompletedTask;
    }
}

[Subscriber(Mode = SubscriberMode.Async)]
public sealed class AsyncEventSubscriber : ISubscriber<AsyncEvent>
{
    public Task HandleAsync(AsyncEvent subscribed, SubscriberContext context, CancellationToken cancellationToken)
    {
        SubscriberRecorder.Calls.Enqueue($"async:{subscribed.Id}:{context.Mode}");
        return Task.CompletedTask;
    }
}

[Subscriber(Mode = SubscriberMode.Async)]
public sealed class FailingEventSubscriber : ISubscriber<FailingEvent>
{
    public Task HandleAsync(FailingEvent subscribed, SubscriberContext context, CancellationToken cancellationToken)
    {
        var exception = new InvalidOperationException("subscriber failure");
        SubscriberRecorder.Exceptions.Enqueue(exception);
        throw exception;
    }
}

public sealed class FailingEventExceptionHandler : ISubscriberExceptionHandler<FailingEvent>
{
    public Task HandleAsync(FailingEvent subscribed, SubscriberContext context, Exception exception, CancellationToken cancellationToken)
    {
        SubscriberRecorder.Calls.Enqueue($"handler:{subscribed.Id}:{context.Mode}");
        return Task.CompletedTask;
    }
}

