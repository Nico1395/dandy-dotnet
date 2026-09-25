namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Represents a read-only wrapper around an event destined for the outbox.
/// </summary>
/// <remarks>
///     <para>
///         The outbox envelope is similar to <see cref="IReadOnlyEnvelope" /> but is specifically designed
///         for events that are published to the outbox for asynchronous processing by subscribers.
///     </para>
///     <para>
///         This interface provides read-only access to outbox envelope properties and is used by subscriber
///         handlers when processing events from the outbox.
///     </para>
/// </remarks>
/// <seealso cref="IReadOnlyEnvelope" />
public interface IReadOnlyOutboxEnvelope
{
    /// <summary>
    ///     Gets the unique identifier of the stream this event belongs to.
    /// </summary>
    /// <value>The stream identifier as a string. This is typically the aggregate ID.</value>
    string StreamId { get; }

    /// <summary>
    ///     Gets the event contained in this outbox envelope.
    /// </summary>
    /// <value>The event object that represents a state change in the system.</value>
    object Event { get; }

    /// <summary>
    ///     Gets the sequential version number of this event within its stream.
    /// </summary>
    /// <value>A 64-bit integer representing the event's position in the stream.</value>
    long Version { get; }

    /// <summary>
    ///     Gets the timestamp when this event was created.
    /// </summary>
    /// <value>A <see cref="DateTime" /> value representing when the event was appended to the event store.</value>
    DateTime Timestamp { get; }

    /// <summary>
    ///     Gets the key of the event type.
    /// </summary>
    /// <value>A string that identifies the event type, as configured by the <see cref="EventAttribute" />.</value>
    string EventKey { get; }

    /// <summary>
    ///     Gets the actual runtime type of the event.
    /// </summary>
    /// <value>A <see cref="Type" /> object representing the runtime type of the event.</value>
    Type RuntimeType { get; }
}
