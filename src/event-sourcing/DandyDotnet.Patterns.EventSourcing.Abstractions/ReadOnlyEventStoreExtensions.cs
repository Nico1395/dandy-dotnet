namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Extension methods for <see cref="IReadOnlyEventStore" /> that provide convenient overloads and utility methods.
/// </summary>
/// <seealso cref="IReadOnlyEventStore" />
/// <seealso cref="EventStoreExtensions" />
public static class ReadOnlyEventStoreExtensions
{
    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     version.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, string streamId, long? toVersion, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId, toVersion, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, string streamId, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId, toVersion: null, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     timestamp.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, string streamId, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId, toVersion: null, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     version.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, object streamId, long? toVersion, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId.ToString() ?? string.Empty, toVersion, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, object streamId, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId.ToString() ?? string.Empty, toVersion: null, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="aggregateType">The type of the aggregate to replay.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     timestamp.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, object streamId, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId.ToString() ?? string.Empty, toVersion: null, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of an aggregate by loading its snapshot and applying subsequent events, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
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
    ///     reconstructed aggregate instance as an <see cref="object" />, or <see langword="null" /> if no events exist
    ///     in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" />, <paramref name="aggregateType" />, or <paramref name="streamId" />
    ///     is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved.
    /// </exception>
    public static async Task<object?> ReplayAggregateAsync(this IReadOnlyEventStore eventStore, Type aggregateType, object streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        return await eventStore.ReplayAggregateAsync(aggregateType, streamId.ToString() ?? string.Empty, toVersion, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
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
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, string streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return (TAggregate?)await eventStore.ReplayAggregateAsync(typeof(TAggregate), streamId, toVersion, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     version.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, string streamId, long? toVersion, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return (TAggregate?)await eventStore.ReplayAggregateAsync(typeof(TAggregate), streamId, toVersion, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     timestamp.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, string streamId, DateTime? toTimestamp, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return (TAggregate?)await eventStore.ReplayAggregateAsync(typeof(TAggregate), streamId, toVersion: null, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, string streamId, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return (TAggregate?)await eventStore.ReplayAggregateAsync(typeof(TAggregate), streamId, toVersion: null, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
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
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, object streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return await eventStore.ReplayAggregateAsync<TAggregate>(streamId.ToString() ?? string.Empty, toVersion, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     version.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, object streamId, long? toVersion, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return await eventStore.ReplayAggregateAsync<TAggregate>(streamId.ToString() ?? string.Empty, toVersion, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to replay events to. If <see langword="null" />, replays to the latest
    ///     timestamp.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, object streamId, DateTime? toTimestamp, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return await eventStore.ReplayAggregateAsync<TAggregate>(streamId.ToString() ?? string.Empty, toVersion: null, toTimestamp, cancellationToken);
    }

    /// <summary>
    ///     Replays the state of a strongly typed aggregate by loading its snapshot and applying subsequent events.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream containing the aggregate's events.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="TAggregate">The type of the aggregate to replay.</typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is the
    ///     reconstructed aggregate instance of type <typeparamref name="TAggregate" />, or <see langword="null" /> if no
    ///     events exist in the stream.
    /// </returns>
    /// <remarks>
    ///     This method provides a strongly typed wrapper around
    ///     <see cref="IReadOnlyEventStore.ReplayAggregateAsync(Type, string, long?, DateTime?, CancellationToken)" />.
    ///     It casts the result to the specified aggregate type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the aggregate factory cannot be resolved or when duplicate events are detected.
    /// </exception>
    public static async Task<TAggregate?> ReplayAggregateAsync<TAggregate>(this IReadOnlyEventStore eventStore, object streamId, CancellationToken cancellationToken)
        where TAggregate : class
    {
        return await eventStore.ReplayAggregateAsync<TAggregate>(streamId.ToString() ?? string.Empty, toVersion: null, toTimestamp: null, cancellationToken);
    }

    /// <summary>
    ///     Replays a stream of events along with its latest snapshot, providing both for direct access.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream to replay.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to include events to. If <see langword="null" />, includes events to
    ///     the end of the stream.
    /// </param>
    /// <param name="toTimestamp">
    ///     The maximum timestamp (inclusive) to include events to. If <see langword="null" />, includes events to
    ///     the end of the stream.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is a tuple
    ///     containing the latest <see cref="IReadOnlySnapshot" /> (<see langword="null" /> if none exists) and an array
    ///     of <see cref="IReadOnlyEnvelope" /> instances representing the events after the snapshot.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This method is a utility that combines the functionality of
    ///         <see cref="IReadOnlyEventStore.GetLastSnapshotAsync" /> and
    ///         <see cref="IReadOnlyEventStore.GetStreamAsync" /> into a single call, returning both the snapshot and
    ///         the stream of events in a conveniently packaged tuple.
    ///     </para>
    ///     <para>
    ///         The returned snapshot is the latest snapshot for the stream that does not exceed the specified
    ///         <paramref name="toVersion" /> and <paramref name="toTimestamp" />. The event stream contains only
    ///         events that occurred after the snapshot was created (if a snapshot exists).
    ///     </para>
    ///     <para>
    ///         This is the most efficient way to rebuild the aggregate state, as it minimizes the number of events
    ///         that need to be replayed by starting from the most recent snapshot.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    public static async Task<(IReadOnlySnapshot? Snapshot, IReadOnlyEnvelope[] Stream)> ReplayStreamAsync(this IReadOnlyEventStore eventStore, string streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        var snapshot = await eventStore.GetLastSnapshotAsync(streamId, toVersion, cancellationToken);
        if (snapshot != null && toTimestamp.HasValue && snapshot.Timestamp > toTimestamp.Value)
            snapshot = null;

        var stream = await eventStore.GetStreamAsync(
            streamId,
            fromVersion: snapshot?.Version + 1,
            toVersion: toVersion,
            fromTimestamp: null,
            toTimestamp: toTimestamp,
            cancellationToken);

        return (snapshot, stream);
    }

    /// <summary>
    ///     Replays a stream of events along with its latest snapshot, without timestamp filtering.
    /// </summary>
    /// <param name="eventStore">The read-only event store to query.</param>
    /// <param name="streamId">The unique identifier of the stream to replay.</param>
    /// <param name="toVersion">
    ///     The maximum version (inclusive) to include events to. If <see langword="null" />, includes events to
    ///     the end of the stream.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the asynchronous operation. The result is a tuple
    ///     containing the latest <see cref="IReadOnlySnapshot" /> (<see langword="null" /> if none exists) and an array
    ///     of <see cref="IReadOnlyEnvelope" /> instances representing the events after the snapshot.
    /// </returns>
    /// <remarks>
    ///     This method provides a convenient overload that omits the timestamp filtering, focusing only
    ///     on version-based filtering.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="eventStore" /> or <paramref name="streamId" /> is <see langword="null" />.
    /// </exception>
    public static Task<(IReadOnlySnapshot? Snapshot, IReadOnlyEnvelope[] Stream)> ReplayStreamAsync(this IReadOnlyEventStore eventStore, string streamId, long? toVersion, CancellationToken cancellationToken)
    {
        return eventStore.ReplayStreamAsync(streamId, toVersion, toTimestamp: null, cancellationToken);
    }
}

