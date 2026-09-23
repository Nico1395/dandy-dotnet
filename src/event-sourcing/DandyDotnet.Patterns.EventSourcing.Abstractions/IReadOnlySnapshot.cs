namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Represents a read-only snapshot of an aggregate's state at a specific point in time.
/// </summary>
/// <remarks>
///     <para>
///         Snapshots are point-in-time representations of an aggregate's state that allow for optimized
///         read operations. Instead of replaying all events from the beginning of a stream, the system can
///         load the snapshot and then apply only the events that occurred after the snapshot was created.
///     </para>
///     <para>
///         This interface provides read-only access to snapshot properties and is returned by event store
///         query methods when snapshots are requested.
///     </para>
/// </remarks>
public interface IReadOnlySnapshot
{
    /// <summary>
    ///     Gets the unique identifier of the stream this snapshot belongs to.
    /// </summary>
    /// <value>The stream identifier as a string. This is typically the aggregate ID.</value>
    string StreamId { get; }

    /// <summary>
    ///     Gets the aggregate instance represented by this snapshot.
    /// </summary>
    /// <value>The aggregate object in its state at the time the snapshot was created.</value>
    /// <remarks>
    ///     The aggregate is stored in its fully materialized form, containing all the state that was
    ///     accumulated up to the version represented by this snapshot.
    /// </remarks>
    object Aggregate { get; }

    /// <summary>
    ///     Gets the version of the stream at which this snapshot was created.
    /// </summary>
    /// <value>A 64-bit integer representing the version at which the snapshot was taken.</value>
    /// <remarks>
    ///     The snapshot version corresponds to the highest version of events that were replayed
    ///     to create this snapshot. Events with versions higher than this value occurred after the
    ///     snapshot and need to be applied to bring the aggregate to its current state.
    /// </remarks>
    long Version { get; }

    /// <summary>
    ///     Gets the timestamp when this snapshot was created.
    /// </summary>
    /// <value>A <see cref="DateTime" /> value representing when the snapshot was stored.</value>
    DateTime Timestamp { get; }

    /// <summary>
    ///     Gets the key of the aggregate type this snapshot belongs to.
    /// </summary>
    /// <value>A string that identifies the aggregate type, as configured by the <see cref="AggregateAttribute" />.</value>
    string AggregateKey { get; }

    /// <summary>
    ///     Gets the actual runtime type of the aggregate.
    /// </summary>
    /// <value>A <see cref="Type" /> object representing the runtime type of the aggregate.</value>
    Type RuntimeType { get; }
}
