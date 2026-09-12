using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

namespace DandyDotnet.Patterns.EventSourcing.Persistence;

public interface ISnapshotRepository
{
    Task<SnapshotEntity?> GetLatestSnapshotAsync(string streamId, long? toVersion, CancellationToken cancellationToken);
    Task InsertAsync(SnapshotEntity snapshotEntity, CancellationToken cancellationToken);
}