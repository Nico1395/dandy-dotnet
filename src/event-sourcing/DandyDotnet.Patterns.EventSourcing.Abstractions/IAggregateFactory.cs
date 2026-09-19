namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Factory interface for creating aggregate instances from snapshots and event streams.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate to create.</typeparam>
/// <remarks>
///     <para>
///         This interface defines the contract for aggregate factories, which are responsible for reconstructing
///         aggregate state from stored snapshots and event streams. Implementations of this interface are used
///         by the event store when replaying aggregate state.
///     </para>
///     <para>
///         The factory receives a snapshot (which may be <see langword="null" /> if no snapshot exists or is available)
///         and an array of <see cref="IReadOnlyEnvelope" /> instances representing the events that need to be applied.
///         The factory should return a fully reconstructed aggregate instance with all events applied.
///     </para>
///     <para>
///         Custom aggregate factories can be registered using the <see cref="AggregateConfigurationBuilder{TAggregate}.UseFactory" />
///         method during event store configuration, or by implementing this interface and registering it
///         with the dependency injection container.
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         internal sealed class UserAggregateFactory : IAggregateFactory<UserAggregate>
///         {
///             public UserAggregate Create(UserAggregate? snapshot, IReadOnlyEnvelope[] envelopes)
///             {
///                 var aggregate = snapshot ?? new UserAggregate();
///                 foreach (var envelope in envelopes)
///                     aggregate.Apply((dynamic)envelope.Event);
/// 
///                 return aggregate;
///             }
///         }
///     </code>
/// </example>
/// <seealso cref="AggregateFactoryAttribute" />
/// <seealso cref="EventStoreExtensions" />
public interface IAggregateFactory<TAggregate>
    where TAggregate : class
{
    /// <summary>
    ///     Creates and initializes an aggregate instance from a snapshot and event stream.
    /// </summary>
    /// <param name="snapshot">
    ///     The latest snapshot of the aggregate, or <see langword="null" /> if no snapshot is available.
    /// </param>
    /// <param name="envelopes">
    ///     The array of event envelopes to apply to the aggregate. The envelopes are ordered by version
    ///     in ascending order (oldest first). This array may be empty but must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The fully reconstructed aggregate instance with all events from the snapshot and envelopes applied.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Implementations should handle the case where <paramref name="snapshot" /> is <see langword="null" /> by
    ///         creating a new instance of the aggregate and applying all events from <paramref name="envelopes" />.
    ///     </para>
    ///     <para>
    ///         When a snapshot is provided, implementations should typically start from that state and then
    ///         apply only the events that occurred after the snapshot was created (these events will have
    ///         versions greater than the snapshot's version).
    ///     </para>
    ///     <para>
    ///         The <see cref="IReadOnlyEnvelope" /> instances contain the event data along with metadata such as
    ///         version, timestamp, and event type. The <see cref="IReadOnlyEnvelope.Event" /> property contains the
    ///         actual event object that should be applied to the aggregate.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="envelopes" /> is <see langword="null" />.
    /// </exception>
    TAggregate Create(TAggregate? snapshot, IReadOnlyEnvelope[] envelopes);
}
