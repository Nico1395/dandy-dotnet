namespace DandyDotnet.Patterns.EventSourcing.Outbox;

public sealed class OutboxEnvelopeConsumer
{
    public required string StreamId { get; init; }
    public required long Version { get; init; }
    public required string ConsumerKey { get; init; }
    public required OutboxEventConsumerType Type { get; init; }
    public DateTime? ConsumedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public int Tries { get; set; }

    internal bool IsNew { get; init; }

    public void Consume()
    {
        Try();
        ConsumedAt = DateTime.UtcNow;
    }

    public void Fail()
    {
        Try();
        FailedAt = DateTime.UtcNow;
    }

    public void Try()
    {
        Tries++;
    }

    public bool CanRetry(int retries)
    {
        return Tries < retries; 
    }
}