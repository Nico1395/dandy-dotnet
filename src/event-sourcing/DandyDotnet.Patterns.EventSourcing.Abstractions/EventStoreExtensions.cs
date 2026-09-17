using System;
using System.Threading;
using System.Threading.Tasks;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Extension methods for <see cref="IEventStore" /> that provide convenient overloads for appending events.
/// </summary>
/// <remarks>
///     <para>
///         These extension methods provide more convenient ways to append events to the event store,
///         with support for generic type arguments and single event appends.
///     </para>
///     <para>
///         The methods are thin wrappers around the <see cref="IEventStore.AppendAsync" /> method and provide
///         the same functionality with different parameter combinations.
///     </para>
/// </remarks>
/// <seealso cref="IEventStore.AppendAsync" />
/// <seealso cref="ReadOnlyEventStoreExtensions" />
public static class EventStoreExtensions
{
    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="aggregateType">The type of the aggregate to which the event belongs.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="@event">The event to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     This method wraps the single event in an array and calls
    ///     <see cref="IEventStore.AppendAsync(Type?, string, object[], CancellationToken)" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="@event" />
    ///     is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, Type aggregateType, string streamId, object @event, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(aggregateType, streamId, [@event], cancellationToken);
    }

    /// <summary>
    ///     Appends multiple events to the specified stream without aggregate type information.
    /// </summary>
    /// <param name="eventStore">The event store to append the events to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the events to.</param>
    /// <param name="events">The array of events to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     This method passes <see langword="null" /> for the aggregate type, which means snapshots will not be
    ///     automatically created for these events.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="events" />
    ///     is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, string streamId, object[] events, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(null, streamId, events, cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream without aggregate type information.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="@event">The event to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     This method wraps the single event in an array, passes <see langword="null" /> for the aggregate type,
    ///     and calls <see cref="IEventStore.AppendAsync(Type?, string, object[], CancellationToken)" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="@event" />
    ///     is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, string streamId, object @event, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(null, streamId, [@event], cancellationToken);
    }

    /// <summary>
    ///     Appends multiple events to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the events to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the events to.</param>
    /// <param name="events">The array of events to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to which the events belong.</typeparam>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     This method provides a strongly-typed way to specify the aggregate type. If snapshots are configured
    ///     for this aggregate type, a snapshot may be automatically created after the events are appended.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="events" />
    ///     is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync<TAggregate>(this IEventStore eventStore, string streamId, object[] events, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return eventStore.AppendAsync(typeof(TAggregate), streamId, events, cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="event">The event to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to which the event belongs.</typeparam>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     This method wraps the single event in an array and calls the generic
    ///     <see cref="AppendAsync{TAggregate}(IEventStore, string, object[], CancellationToken)" /> method.
    ///     If snapshots are configured for this aggregate type, a snapshot may be automatically created after
    ///     the event is appended.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="@event" />
    ///     is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync<TAggregate>(this IEventStore eventStore, string streamId, object @event, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return eventStore.AppendAsync(typeof(TAggregate), streamId, [@event], cancellationToken);
    }
}
