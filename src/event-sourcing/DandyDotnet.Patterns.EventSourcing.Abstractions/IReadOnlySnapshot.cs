namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IReadOnlySnapshot
{
    string StreamId { get; }
    object Aggregate { get; }
    long Version { get; }
    DateTime Timestamp { get; }
    string AggregateKey { get; }
    Type RuntimeType { get; }
}