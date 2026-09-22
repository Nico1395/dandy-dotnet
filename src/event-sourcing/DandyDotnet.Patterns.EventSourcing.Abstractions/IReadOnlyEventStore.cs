using System;
using System.Threading;
using System.Threading.Tasks;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Represents a read-only event store that can query and replay events from streams.
/// </summary>
/// <remarks>
///     <para>
///         The read-only event store provides the ability to read events from streams without modifying them.
///         It supports replaying aggregate state from stored events and snapshots, as well as querying
///         event streams with various filtering options.
///     </para>
///     <para>
///         This interface is implemented by <see cref="IEventStore" /> and can be used when only read operations
///         are needed, such as in query handlers or read models.
///     </para>
/// </remarks>
/// <seealso cref="IEventStore" />
/// <seealso cref="ReadOnlyEventStoreExtensions" />
public interface IReadOnlyEventStore
{
    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot (if available) and applying subsequent events.
    /// </summary>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     version.
    /// </param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     timestamp.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance, or <see langword="null" /> if no events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This method optimizes read performance by first loading the latest snapshot (if available and
    ///         not excluded by <paramref name="toVersion" /> or <paramref name="toTimestamp" />), then applying
    ///         only the events that occurred after the snapshot was created.
    ///     </para>
    ///     <para>
    ///         The aggregate is reconstructed using the configured aggregate factory for the specified
    ///         <paramref name="aggregateType" />. If no factory is configured, or if the factory cannot be resolved
    ///         from the service provider, an exception is thrown.
    ///     </para>
    ///     <para>
    ///         If the stream does not exist or contains no events, this method returns <see langword="null" />.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="aggregateType" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    /// <seealso cref="ReadOnlyEventStoreExtensions.ReplayAggregateAsync{TAggregate}(IReadOnlyEventStore, string, long?, DateTime?, CancellationToken)" />
    /// <seealso cref="ReadOnlyEventStoreExtensions.ReplayStreamAsync(IReadOnlyEventStore, string, long?, DateTime?, CancellationToken)" />
    Task<object?> ReplayAggregateAsync(Type aggregateType, string streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken);

    /// <summary>
    ///     Gets all events from a specific stream, optionally filtered by version and/or timestamp range.
    /// </summary>
    /// <param name="streamId">The unique identifier of the stream to query.</param>
    /// <param name="fromVersion">
    ///     The minimum version (inclusive) to include events from. If <see langword="null" />, includes events from
    ///     the beginning of the stream.
    /// </param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to include events to. If <see langword="null" />, includes events to the
    ///     end of the stream.
    /// </param>
    /// <param name="fromTimestamp">
    ///     The minimum timestamp (inclusive) to include events from. If <see langword="null" />, includes events from
    ///     the beginning of the stream.
    /// </param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to include events to. If <see langword="null" />, includes events to the
    ///     end of the stream.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is an array
    ///     of <see cref="IReadOnlyEnvelope" /> instances containing the requested events.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         The returned envelopes are ordered by version in ascending order (oldest first).
    ///         The array may be empty if no events match the specified criteria.
    ///     </para>
    ///     <para>
    ///         This method does not include snapshots. To get snapshots, use
    ///         <see cref="GetLastSnapshotAsync(string, long?, CancellationToken)" />.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    Task<IReadOnlyEnvelope[]> GetStreamAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken);

    /// <summary>
    ///     Gets the latest snapshot for a stream, optionally limited by maximum version.
    /// </summary>
    /// <param name="streamId">The unique identifier of the stream to query.</param>
    /// <param name="version">
    ///     The maximum version (inclusive) of the snapshot to retrieve. If <see langword="null" />, retrieves
    ///     the latest snapshot regardless of version.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     <see cref="IReadOnlySnapshot" /> instance containing the snapshot, or <see langword="null" /> if no snapshot
    ///     exists or if snapshots are not enabled for the aggregate.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Snapshots are only returned if the aggregate associated with the stream has snapshots enabled
    ///         in its configuration. If snapshots are not configured for the aggregate, this method returns
    ///         <see langword="null" />.
    ///     </para>
    ///     <para>
    ///         The snapshot contains the state of the aggregate at a specific point in time, which can be used
    ///         to optimize read operations by avoiding the need to replay all events from the beginning.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    Task<IReadOnlySnapshot?> GetLastSnapshotAsync(string streamId, long? version, CancellationToken cancellationToken);

    /// <summary>
    ///     Gets envelopes stored with at least one of the given <paramref name="tags"/>.
    /// </summary>
    /// <param name="tags">
    ///     Tags to query for.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     Envelopes that have been stored with at least one matching tag from <paramref name="tags"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Matching tags is case-sensitive. <see langword="null"/>-tags are filtered out when storing. Storing
    ///         events with tags containing the delimiter <c>;</c> is prevented with an exception.
    ///     </para>
    /// </remarks>
    Task<IReadOnlyEnvelope[]> GetEnvelopesAsync(IEnumerable<string> tags, CancellationToken cancellationToken);
}
