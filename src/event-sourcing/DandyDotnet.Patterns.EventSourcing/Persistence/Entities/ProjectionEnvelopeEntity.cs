namespace DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

public sealed class ProjectionEnvelopeEntity
{
    public required string Key { get; init; }
    public required string StreamId { get; init; }
    public required string Payload { get; init; }
    public required string ProjectionKey { get; init; }
    public required long Version { get; init; }
    public required DateTime Timestamp { get; init; }
}