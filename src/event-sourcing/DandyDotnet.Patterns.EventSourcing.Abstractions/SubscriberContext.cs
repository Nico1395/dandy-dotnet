namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Provides context information to subscribers during event processing.
/// </summary>
/// <remarks>
///     <para>
///         The <see cref="SubscriberContext" /> is passed to <see cref="ISubscriber{TEvent}.HandleAsync" /> and
///         <see cref="ISubscriberExceptionHandler{TEvent}.HandleAsync" /> methods to provide additional information
///         about the event being processed, the event store, and the retry state.
///     </para>
///     <para>
///         This context is immutable and is created by the event sourcing system before invoking the
///         subscriber. It contains all the metadata needed for proper event processing and error handling.
///     </para>
/// </remarks>
/// <seealso cref="ISubscriber{TEvent}" />
/// <seealso cref="ISubscriberExceptionHandler{TEvent}" />
public sealed class SubscriberContext
{
    /// <summary>
    ///     Gets the event store instance that triggered this subscriber.
    /// </summary>
    /// <value>An <see cref="IEventStore" /> instance that can be used to query or append events.</value>
    /// <remarks>
    ///     This property provides access to the event store, allowing subscribers to perform additional
    ///     queries or even append new events as part of their processing. Note that appending events
    ///     from within a subscriber may create complex scenarios and should be used with caution.
    /// </remarks>
    public required IEventStore EventStore { get; init; }

    /// <summary>
    ///     Gets the envelope containing the event being processed.
    /// </summary>
    /// <value>An <see cref="IReadOnlyOutboxEnvelope" /> containing the event and its metadata.</value>
    /// <remarks>
    ///     The envelope provides access to the event's metadata, including the stream ID, version,
    ///     timestamp, and event type information. This is the same envelope that was published to the
    ///     outbox by the event store.
    /// </remarks>
    public required IReadOnlyOutboxEnvelope Envelope { get; init; }

    /// <summary>
    ///     Gets the mode in which this subscriber is being executed.
    /// </summary>
    /// <value>A <see cref="SubscriberMode" /> value indicating whether this is inline or async processing.</value>
    /// <remarks>
    ///     This indicates whether the subscriber was triggered synchronously during an append operation
    ///     (<see cref="SubscriberMode.Inline" />) or asynchronously by the outbox daemon
    ///     (<see cref="SubscriberMode.Async" />).
    /// </remarks>
    public required SubscriberMode Mode { get; init; }

    /// <summary>
    ///     Gets the maximum number of retries configured for this subscriber.
    /// </summary>
    /// <value>An integer representing the maximum retry count, or -1 if retries are unlimited.</value>
    /// <remarks>
    ///     This value comes from the subscriber configuration, either from the <see cref="SubscriberAttribute.Retries" />
    ///     or from the global configuration. A value of -1 means the subscriber will be retried indefinitely.
    /// </remarks>
    public required int MaxRetries { get; init; }

    /// <summary>
    ///     Gets the current retry count for this subscriber execution.
    /// </summary>
    /// <value>An integer starting at 0 for the first attempt, and incrementing with each retry.</value>
    /// <remarks>
    ///     This count indicates how many times this particular event has been attempted by this subscriber.
    ///     A value of 0 means this is the first attempt.
    /// </remarks>
    public required int RetryCount { get; init; }

    /// <summary>
    ///     Determines whether this subscriber can be retried again.
    /// </summary>
    /// <param name="retries">The maximum number of retries to compare against.</param>
    /// <returns>
    ///     <see langword="true" /> if <see cref="RetryCount" /> is less than <paramref name="retries" />;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This helper method can be used to check if the subscriber has remaining retry attempts
    ///     based on a specified maximum retry count. It is equivalent to checking if
    ///     <see cref="RetryCount" /> &lt; <paramref name="retries" />.
    /// </remarks>
    public bool CanRetry(int retries)
    {
        return RetryCount < retries;
    }

    /// <summary>
    ///     Determines whether this is the first attempt at processing this event.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if <see cref="RetryCount" /> is 0 (first attempt); otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This helper method provides a convenient way to check if this is the initial processing
    ///     attempt, which can be useful for logging or special first-attempt behavior.
    /// </remarks>
    public bool IsFirstTry()
    {
        return RetryCount == 0;
    }
}
