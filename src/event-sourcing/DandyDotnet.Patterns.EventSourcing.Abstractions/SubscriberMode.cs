namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Specifies the execution mode for a subscriber.
/// </summary>
/// <remarks>
///     <para>
///         The subscriber mode determines when and how the subscriber is invoked to process events.
///         There are two modes available:
///         <list type="table">
///             <listheader>
///                 <term>Mode</term>
///                 <description>Description</description>
///             </listheader>
///             <item>
///                 <term><see cref="Inline" /></term>
///                 <description>Subscribers are invoked synchronously during the append operation, before the transaction is committed.</description>
///             </item>
///             <item>
///                 <term><see cref="Async" /></term>
///                 <description>Subscribers are invoked asynchronously by the outbox daemon, after the transaction is committed.</description>
///             </item>
///         </list>
///     </para>
///     <para>
///         Inline subscribers are processed immediately when events are appended to the event store.
///         If an inline subscriber fails, the append operation will fail. Async subscribers are processed
///         by the background outbox daemon and do not block the append operation.
///     </para>
/// </remarks>
/// <seealso cref="SubscriberAttribute.Mode" />
/// <seealso cref="SubscriberContext.Mode" />
public enum SubscriberMode
{
    /// <summary>
    ///     Inline subscriber mode. Subscribers are invoked synchronously during the append operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Inline subscribers are processed immediately before the transaction is committed. This ensures
    ///         that the subscriber sees the events in the same transaction as they are stored.
    ///     </para>
    ///     <para>
    ///         If an inline subscriber throws an exception, the entire append operation will fail, and
    ///         no events will be persisted to the event store.
    ///     </para>
    ///     <para>
    ///         Use this mode when the subscriber needs to perform operations that must be atomic with
    ///         the event storage, or when immediate processing is required.
    ///     </para>
    /// </remarks>
    Inline = 0,

    /// <summary>
    ///     Asynchronous subscriber mode. Subscribers are invoked asynchronously by the outbox daemon.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Async subscribers are processed by the background outbox daemon after the events have been
    ///         committed to the event store. This provides better performance for long-running operations
    ///         as it does not block the append operation.
    ///     </para>
    ///     <para>
    ///         If an async subscriber fails, the failure will be handled by the configured retry logic and
    ///         exception handlers. The append operation will not be affected by async subscriber failures.
    ///     </para>
    ///     <para>
    ///         Use this mode for most subscribers, especially those that perform I/O operations like sending
    ///         notifications, writing to external systems, or other non-critical operations.
    ///     </para>
    /// </remarks>
    Async = 1,
}
