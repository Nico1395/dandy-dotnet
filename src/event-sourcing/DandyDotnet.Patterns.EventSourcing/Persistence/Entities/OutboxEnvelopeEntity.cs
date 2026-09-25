using DandyDotnet.Patterns.EventSourcing.Outbox;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

/// <summary>
/// Database entity for storing serialized outbox envelopes.
/// </summary>
public sealed class OutboxEnvelopeEntity
{
    /// <summary>
    /// Stream ID the envelope's event belongs to.
    /// </summary>
    public required string StreamId { get; init; }
    
    /// <summary>
    /// Serialized event payload.
    /// </summary>
    public required string Payload { get; init; }
    
    /// <summary>
    /// Version of the <see cref="Envelope.Event"/>.
    /// </summary>
    public required long Version { get; init; }
    
    /// <summary>
    /// Timestamp of when the event was created.
    /// </summary>
    public required DateTime Timestamp { get; init; }
    
    /// <summary>
    /// Key of the event type to resolve the <see cref="OutboxEnvelope.RuntimeType"/>.
    /// </summary>
    public required string EventKey { get; init; }
    
    /// <summary>
    /// Consumer entries of the envelope.
    /// </summary>
    public required List<OutboxEnvelopeConsumerEntity> Consumers { get; init; } = [];
}