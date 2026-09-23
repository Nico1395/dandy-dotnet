namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Represents a read-only wrapper around an event stored in the event store.
/// </summary>
/// <remarks>
///     <para>
///         An envelope wraps an event with metadata that is required for proper event sourcing functionality.
///         This includes the stream identifier, version number, timestamp, and event type information.
///     </para>
///     <para>
///         This interface provides read-only access to envelope properties and is returned by event store
///         query methods. It is the primary data structure used when replaying aggregate state.
///     </para>
/// </remarks>
public interface IReadOnlyEnvelope
{
    /// <summary>
    ///     Gets the unique identifier of the stream this event belongs to.
    /// </summary>
    /// <value>The stream identifier as a string. This is typically the aggregate ID.</value>
    /// <remarks>
    ///     The stream ID uniquely identifies the sequence of events for a particular aggregate instance.
    ///     All events for a single aggregate share the same stream ID.
    /// </remarks>
    string StreamId { get; }

    /// <summary>
    ///     Gets the event contained in this envelope.
    /// </summary>
    /// <value>The event object that represents a state change in the system.</value>
    /// <remarks>
    ///     The event object is the actual domain event that occurred. It can be of any type, as defined
    ///     by the <see cref="RuntimeType" /> property.
    /// </remarks>
    object Event { get; }

    /// <summary>
    ///     Gets the sequential version number of this event within its stream.
    /// </summary>
    /// <value>A 64-bit integer representing the event's position in the stream.</value>
    /// <remarks>
    ///     <para>
    ///         Version numbers start at 0 for the first event in a stream and increment by 1 for each
    ///         subsequent event. The version is used to ensure proper ordering when replaying events.
    ///     </para>
    ///     <para>
    ///         Within a single stream, version numbers are unique and monotonically increasing.
    ///     </para>
    /// </remarks>
    long Version { get; }

    /// <summary>
    ///     Gets the timestamp when this event was created.
    /// </summary>
    /// <value>A <see cref="DateTime" /> value representing when the event was appended to the event store.</value>
    /// <remarks>
    ///     The timestamp is in UTC and is set by the event store when the event is appended.
    ///     It can be used for temporal queries and event expiration logic.
    /// </remarks>
    DateTime Timestamp { get; }

    /// <summary>
    ///     Gets the key of the event type.
    /// </summary>
    /// <value>A string that identifies the event type, as configured by the <see cref="EventAttribute" />.</value>
    /// <remarks>
    ///     This key is used for routing events to the correct handlers and for event type identification
    ///     during serialization and deserialization.
    /// </remarks>
    string EventKey { get; }

    /// <summary>
    ///     Gets the actual runtime type of the event.
    /// </summary>
    /// <value>A <see cref="Type" /> object representing the runtime type of the event.</value>
    /// <remarks>
    ///     This property contains the actual .NET type of the event object, which can be used for
    ///     type-based operations such as casting or reflection.
    /// </remarks>
    Type RuntimeType { get; }

    /// <summary>
    ///     Gets tags the event was associated with when appending to its respective stream.
    /// </summary>
    /// <remarks>
    ///     Tags are always normalized on inserting and will never be <see langword="null"/>.
    /// </remarks>
    string[] Tags { get; }
}
