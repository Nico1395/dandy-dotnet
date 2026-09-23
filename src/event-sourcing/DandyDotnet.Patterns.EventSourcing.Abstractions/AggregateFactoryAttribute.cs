namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Attribute that marks a method or constructor as an aggregate factory method.
/// </summary>
/// <remarks>
///     <para>
///         This attribute is used to identify factory methods that create aggregate instances from snapshots
///         and event streams. The method should accept a snapshot and an array of events, and return the
///         reconstructed aggregate.
///     </para>
///     <para>
///         When applied to a method, the method signature should match the <see cref="IAggregateFactory{T}" />
///         interface's <c>Create</c> method: accepting a nullable snapshot and an array of <see cref="IReadOnlyEnvelope" />.
///     </para>
///     <para>
///         This allows the event sourcing system to automatically discover and use custom factory methods
///         instead of relying solely on the default <see cref="IAggregateFactory{T}" /> interface implementations.
///     </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public sealed class AggregateFactoryAttribute : Attribute;
