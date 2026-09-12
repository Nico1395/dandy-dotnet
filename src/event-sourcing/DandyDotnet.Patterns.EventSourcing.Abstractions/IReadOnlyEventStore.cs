namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IReadOnlyEventStore
{
    Task<object?> ReplayAggregateAsync(Type aggregateType, string streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken);
    Task<IReadOnlyEnvelope[]> GetStreamAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken);
    Task<IReadOnlySnapshot?> GetLastSnapshotAsync(string streamId, long? version, CancellationToken cancellationToken);
}