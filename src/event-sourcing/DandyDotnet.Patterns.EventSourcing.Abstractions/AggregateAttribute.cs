using System;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Attribute that marks a class as an aggregate root in the Event Sourcing pattern.
/// </summary>
/// <remarks>
///     <para>
///         Aggregate roots are the consistency boundary in Domain-Driven Design. This attribute allows configuration
///         of the aggregate's key and snapshot behavior.
///     </para>
///     <para>
///         When applied to a class, it indicates that the class represents an aggregate root that can be
///         event-sourced. The <see cref="Key" /> property defines how the aggregate is identified in the event store,
///         and <see cref="SnapshotInterval" /> controls snapshot creation frequency.
///     </para>
///     <para>
///         If <see cref="SnapshotInterval" /> is set to a positive value, snapshots will be automatically created
///         after the specified number of events have been appended to the aggregate's stream.
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         [Aggregate(Key = "User", SnapshotInterval = 10)]
///         public class UserAggregate
///         {
///             // Aggregate implementation
///         }
///     </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public sealed class AggregateAttribute : Attribute
{
    /// <summary>
    ///     Gets the key used to identify this aggregate type in the event store.
    /// </summary>
    /// <value>
    ///     A string key that uniquely identifies the aggregate type. If <see langword="null" />, the aggregate's
    ///     type name will be used as the default key.
    /// </value>
    /// <remarks>
    ///     This key is used for routing and identification purposes within the event store. It allows
    ///     the system to look up aggregate configurations and map events to their respective aggregates.
    /// </remarks>
    public string? Key { get; init; }

    /// <summary>
    ///     Gets the interval at which snapshots are created for this aggregate.
    /// </summary>
    /// <value>
    ///     The number of events after which a snapshot should be created. A value of -1 (the default) means
    ///     snapshots are disabled for this aggregate.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         Snapshots improve read performance by allowing the system to rebuild an aggregate from a saved
    ///         state rather than replaying all events from the beginning. This is especially useful for aggregates
    ///         with long event histories.
    ///     </para>
    ///     <para>
    ///         A snapshot is automatically created after every N events, where N is the value of this property.
    ///         For example, if set to 10, a snapshot will be created after every 10 events are appended to the stream.
    ///     </para>
    /// </remarks>
    public int SnapshotInterval { get; init; } = -1;
}
