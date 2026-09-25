using DandyDotnet.Patterns.EventSourcing.Outbox;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

/// <summary>
/// Database entity for storing outbox envelope consumers and their status.
/// </summary>
public sealed class OutboxEnvelopeConsumerEntity
{
    /// <summary>
    /// ID of the stream the <see cref="OutboxEnvelopeEntity"/> belongs to.
    /// </summary>
    public required string StreamId { get; init; }
    
    /// <summary>
    /// Version of the <see cref="OutboxEnvelopeEntity"/>.
    /// </summary>
    public required long Version { get; init; }
    
    /// <summary>
    /// Unique consumer key.
    /// </summary>
    public required string ConsumerKey { get; init; }
    
    /// <summary>
    /// Type of the consumer.
    /// </summary>
    public required OutboxEventConsumerType Type { get; init; }
    
    /// <summary>
    /// Timestamp of when the consumer has successfully consumed the <see cref="OutboxEnvelope.Event"/>.
    /// </summary>
    public DateTime? ConsumedAt { get; set; }
    
    /// <summary>
    /// Timestamp of when the consumer failed consumption.
    /// </summary>
    public DateTime? FailedAt { get; set; }
    
    /// <summary>
    /// Amount of consumption attempts.
    /// </summary>
    public int Tries { get; set; }
}