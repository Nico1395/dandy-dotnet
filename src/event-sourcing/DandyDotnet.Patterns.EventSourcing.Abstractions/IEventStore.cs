namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Represents a writable event store that can append events to streams and read from them.
/// </summary>
/// <remarks>
///     <para>
///         The event store is the central component of the Event Sourcing pattern. It provides a durable,
///         append-only log of events that represent state changes in the system.
///     </para>
///     <para>
///         This interface extends <see cref="IReadOnlyEventStore" /> to add write capabilities. It provides
///         methods for appending events to streams, which allows building up the event history of aggregates.
///     </para>
///     <para>
///         When events are appended, they are stored durably and then published to the outbox for
///         asynchronous processing by subscribers. The event store ensures atomicity between storing
///         events and publishing them to the outbox.
///     </para>
/// </remarks>
/// <seealso cref="IReadOnlyEventStore" />
/// <seealso cref="EventStoreExtensions" />
public interface IEventStore : IReadOnlyEventStore
{
    /// <summary>
    ///     Appends the specified events to the stream with the given ID.
    /// </summary>
    /// <param name="aggregateType">
    ///     The type of the aggregate to which the events belong, or <see langword="null" /> if the events are not
    ///     associated with a specific aggregate type.
    /// </param>
    /// <param name="streamId">The unique identifier of the stream to append the events to.</param>
    /// <param name="events">Collection of events with optional tags.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     <para>
    ///         This method is atomic: all events are appended together in a single transaction. If the
    ///         operation fails, none of the events will be persisted.
    ///     </para>
    ///     <para>
    ///         The events are assigned sequential version numbers starting from the current version of the stream.
    ///         The first event will have version N+1 where N is the current highest version of the stream.
    ///     </para>
    ///     <para>
    ///         After the events are stored, they are published to the outbox for asynchronous processing by
    ///         registered subscribers. Inline subscribers are notified immediately after the transaction
    ///         is committed.
    ///     </para>
    ///     <para>
    ///         If <paramref name="aggregateType" /> is provided and snapshots are configured for that aggregate,
    ///         a snapshot may be automatically created after the events are appended, depending on the
    ///         snapshot interval configuration.
    ///     </para>
    ///     <para>
    ///         Tags of value <see langword="null"/> and duplicates will be filtered out. Tags will be normalized to invariant
    ///         lower-case strings. Tags will be sorted alphabetically.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="streamId" /> is <see langword="null" />, empty, or whitespace.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when duplicate events are detected for the same version in a stream.
    /// </exception>
    /// <seealso cref="EventStoreExtensions.AppendAsync(IEventStore, Type, string, object, CancellationToken)" />
    /// <seealso cref="EventStoreExtensions.AppendAsync(IEventStore, string, object[], CancellationToken)" />
    Task AppendAsync(Type? aggregateType, string streamId, (object Event, IEnumerable<string>? Tags)[] events, CancellationToken cancellationToken);
}
