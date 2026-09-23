namespace DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

/// <summary>
/// Database entity for storing serialized snapshots with metadata.
/// </summary>
public sealed class SnapshotEntity
{
    /// <summary>
    /// ID of the stream the snapshot belongs to.
    /// </summary>
    public required string StreamId { get; init; }
    
    /// <summary>
    /// Serialized aggregate payload.
    /// </summary>
    public required string Payload { get; init; }
    
    /// <summary>
    /// Key of the aggregate type to resolve the <see cref="Snapshot.RuntimeType"/> of the target aggregate.
    /// </summary>
    public required string AggregateKey { get; init; }
    
    /// <summary>
    /// Version of the snapshot.
    /// </summary>
    public required long Version { get; init; }
    
    /// <summary>
    /// Timestamp of when the snapshot was taken.
    /// </summary>
    public required DateTime Timestamp { get; init; }
}