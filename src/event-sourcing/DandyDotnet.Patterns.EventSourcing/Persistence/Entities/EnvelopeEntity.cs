namespace DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

/// <summary>
/// Database entity for storing serialized events with envelope metadata.
/// </summary>
public sealed class EnvelopeEntity
{
    /// <summary>
    /// ID of the stream the <see cref="Payload"/> belongs to.
    /// </summary>
    public required string StreamId { get; init; }
    
    /// <summary>
    /// The serialized event payload.
    /// </summary>
    public required string Payload { get; init; }
    
    /// <summary>
    /// Version of the event.
    /// </summary>
    public required long Version { get; init; }
    
    /// <summary>
    /// Timestamp of when the event was created.
    /// </summary>
    public required DateTime Timestamp { get; init; }
    
    /// <summary>
    /// Key of the event type for resolving the <see cref="Envelope.RuntimeType"/>.
    /// </summary>
    public required string EventKey { get; init; }
    
    /// <summary>
    /// Tags to associate the event with.
    /// </summary>
    public required string[] Tags { get; init; }
}