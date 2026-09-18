namespace DandyDotnet.Patterns.EventSourcing.Projections;

internal sealed class ProjectionEnvelope
{
    public required string Key { get; init; }
    public required string StreamId { get; init; }
    public required object Projection { get; init; }
    public required Type RuntimeType { get; init; }
    public required string ProjectionKey { get; init; }
    public required long Version { get; init; }
    public required DateTime Timestamp { get; init; }
}