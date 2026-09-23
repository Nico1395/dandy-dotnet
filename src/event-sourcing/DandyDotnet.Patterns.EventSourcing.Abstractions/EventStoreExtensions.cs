using System;
using System.Threading;
using System.Threading.Tasks;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Extension methods for <see cref="IEventStore" /> that provide convenient overloads for appending events.
/// </summary>
/// <seealso cref="ReadOnlyEventStoreExtensions" />
public static class EventStoreExtensions
{
    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="aggregateType">The type of the aggregate to which the event belongs.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="event">The event to append to the stream.</param>
    /// <param name="tags">The tags to associate the event with.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="event" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, Type? aggregateType, string streamId, object @event, IEnumerable<string>? tags, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(
            aggregateType,
            streamId,
            events: [(@event, tags)],
            cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="aggregateType">The type of the aggregate to which the event belongs.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="events">The events to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, Type? aggregateType, string streamId, object[] events, CancellationToken cancellationToken)
    {
        var mapped = events
            .Select<object, (object Event, IEnumerable<string>? Tags)>(e => (e, null))
            .ToArray();

        return eventStore.AppendAsync(
            aggregateType,
            streamId,
            events: mapped,
            cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="aggregateType">The type of the aggregate to which the event belongs.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="event">The event to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="event" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, Type? aggregateType, string streamId, object @event, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(
            aggregateType,
            streamId,
            @event,
            tags: null,
            cancellationToken);
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
    ///     <para>
    ///         Does not provide an aggregate type and thus will not produce a snapshot.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, string streamId, object[] events, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(
             aggregateType: null,
             streamId,
             events,
             cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream without aggregate type information.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="event">The event to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <remarks>
    ///     <para>
    ///         Does not provide an aggregate type and thus will not produce a snapshot.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="event" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync(this IEventStore eventStore, string streamId, object @event, CancellationToken cancellationToken)
    {
        return eventStore.AppendAsync(
            aggregateType: null,
            streamId,
            @event,
            tags: null,
            cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="events">The events to append to the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to which the events belong.</typeparam>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync<TAggregate>(this IEventStore eventStore, string streamId, (object Event, IEnumerable<string>? Tags)[] events, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return eventStore.AppendAsync(
            typeof(TAggregate),
            streamId,
            events,
            cancellationToken);
    }

    /// <summary>
    ///     Appends a single event to the specified stream with the given aggregate type.
    /// </summary>
    /// <param name="eventStore">The event store to append the event to.</param>
    /// <param name="streamId">The unique identifier of the stream to append the event to.</param>
    /// <param name="event">The event to append to the stream.</param>
    /// <param name="tags">The tags to associate the event with.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to which the events belong.</typeparam>
    /// <returns>A <see cref="Task" /> that represents the asynchronous append operation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="event" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync<TAggregate>(this IEventStore eventStore, string streamId, object @event, IEnumerable<string>? tags, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return eventStore.AppendAsync(
            typeof(TAggregate),
            streamId,
            events: [(@event, tags)],
            cancellationToken);
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
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="events" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync<TAggregate>(this IEventStore eventStore, string streamId, object[] events, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return eventStore.AppendAsync(
            typeof(TAggregate),
            streamId,
            events,
            cancellationToken);
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
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="streamId" />, or <paramref name="event" /> is <see langword="null" />.
    /// </exception>
    public static Task AppendAsync<TAggregate>(this IEventStore eventStore, string streamId, object @event, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return eventStore.AppendAsync(
            typeof(TAggregate),
            streamId,
            @event,
            tags: null,
            cancellationToken);
    }
}
