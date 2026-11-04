using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

namespace DandyDotnet.Patterns.EventSourcing.Persistence;

/// <summary>
/// Reads and writes snapshots from and to the event store database.
/// </summary>
public interface ISnapshotRepository
{
    /// <summary>
    /// Gets the closest latest snapshot entity of the stream with ID <paramref name="streamId"/> to <paramref name="toVersion"/>.
    /// </summary>
    /// <param name="streamId">The ID of the event stream of the snapshot.</param>
    /// <param name="toVersion">Optional version to get the closest snapshot for.</param>
    /// <param name="cancellationToken">For cancelling asynchronous operations.</param>
    /// <returns>The snapshot entity, if existing.</returns>
    Task<SnapshotEntity?> GetLatestSnapshotAsync(string streamId, long? toVersion, CancellationToken cancellationToken);

    /// <summary>Gets the latest matching snapshot for each stream.</summary>
    /// <param name="streamIds">Stream IDs to query; null selects all streams and an empty array selects none.</param>
    /// <param name="aggregateKey">Aggregate type key to match, or null for all aggregate types.</param>
    /// <param name="cancellationToken">For cancelling asynchronous operations.</param>
    /// <returns>Snapshots found; streams without a matching snapshot are omitted.</returns>
    Task<Dictionary<string, SnapshotEntity>> GetLatestSnapshotsAsync(string[]? streamIds, string? aggregateKey, CancellationToken cancellationToken);
    
    /// <summary>
    /// Inserts the given <paramref name="snapshotEntity"/>.
    /// </summary>
    /// <param name="snapshotEntity">The snapshot entity to insert.</param>
    /// <param name="cancellationToken">For cancelling asynchronous operations.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    Task InsertAsync(SnapshotEntity snapshotEntity, CancellationToken cancellationToken);
}