namespace DandyDotnet.Patterns.EventSourcing.Outbox;

/// <summary>
/// Abstraction of the outbox pattern for the event store.
/// </summary>
/// <remarks>
///     <para>
///         Only <c>public</c> for unit tests. Not intended for use outside of event sourcing.
///     </para>
/// </remarks>
public interface IOutbox
{
    /// <summary>
    /// Publishes given <paramref name="outboxEnvelopes"/> to the outbox.
    /// </summary>
    /// <param name="outboxEnvelopes">Outbox envelopes to publish.</param>
    /// <param name="cancellationToken">Token for cancelling asynchronous operations.</param>
    /// <returns>Task representing the operation.</returns>
    /// <remarks>
    ///     <para>
    ///         Only stores the envelopes to the outbox. Does not notify consumers.
    ///     </para>
    /// </remarks>
    Task PublishAsync(OutboxEnvelope[] outboxEnvelopes, CancellationToken cancellationToken);
    
    /// <summary>
    /// Notifies all inline consumers for the given <paramref name="outboxEnvelopes"/> and stores their results as
    /// consumer entries.
    /// </summary>
    /// <param name="outboxEnvelopes">Outbox envelopes to notify about.</param>
    /// <param name="cancellationToken">Token for cancelling asynchronous operations.</param>
    /// <returns>Task representing the operation.</returns>
    Task NotifyInlineConsumersAsync(OutboxEnvelope[] outboxEnvelopes, CancellationToken cancellationToken);

    /// <summary>
    /// Queries all outbox envelopes and processes them asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Token for cancelling asynchronous operations.</param>
    /// <returns>Task representing the operation.</returns>
    /// <remarks>
    ///     <para>
    ///         Should only be called during an iteration of the async daemon.
    ///     </para>
    ///     <para>
    ///         Orchestrates asynchronous operations of the outbox. That means it queries all envelopes, deletes expired
    ///         envelopes, notifies consumers, and stores their results as consumer entries to keep track of consumption.
    ///     </para>
    /// </remarks>
    Task NotifyAsyncSubscribersAsync(CancellationToken cancellationToken);
}