using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing;

internal sealed class Envelope : IReadOnlyEnvelope
{
    public required string StreamId { get; init; }
    public required object Event { get; init; }
    public required long Version { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string EventKey { get; init; }
    public required Type RuntimeType { get; init; }
}