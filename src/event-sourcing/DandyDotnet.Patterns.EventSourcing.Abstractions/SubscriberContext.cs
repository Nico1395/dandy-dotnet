namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public sealed class SubscriberContext
{
    public required IEventStore EventStore { get; init; }
    public required IReadOnlyOutboxEnvelope Envelope { get; init; }
    public required SubscriberMode Mode { get; init; }
    public required int MaxRetries { get; init; }
    public required int RetryCount { get; init; }

    public bool CanRetry(int retries)
    {
        return RetryCount < retries;
    }

    public bool IsFirstTry()
    {
        return RetryCount == 0;
    }
}