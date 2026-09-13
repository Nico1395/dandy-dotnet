namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IReadOnlyOutboxEnvelope
{
    string StreamId { get; }
    object Event { get; }
    long Version { get; }
    DateTime Timestamp { get; }
    string EventKey { get; }
    Type RuntimeType { get; }
}