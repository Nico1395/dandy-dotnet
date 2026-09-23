using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

namespace DandyDotnet.Patterns.EventSourcing.Persistence;

/// <summary>
/// Repository for outbox-related database operations.
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Gets all outbox envelopes.
    /// </summary>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>The found outbox envelopes.</returns>
    Task<OutboxEnvelopeEntity[]> GetEnvelopesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Inserts the given <paramref name="envelopes"/>.
    /// </summary>
    /// <param name="envelopes">The envelopes to insert.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>Task representing the operation.</returns>
    Task InsertEnvelopesAsync(OutboxEnvelopeEntity[] envelopes, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the given <paramref name="envelopes"/>.
    /// </summary>
    /// <param name="envelopes">The envelopes to delete.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>Task representing the operation.</returns>
    /// <remarks>
    ///     <para>
    ///         Also deletes the consumer entries of all envelopes.
    ///     </para>
    /// </remarks>
    Task DeleteEnvelopesAsync(OutboxEnvelopeEntity[] envelopes, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts the given <paramref name="consumers"/>.
    /// </summary>
    /// <param name="consumers">Consumers to insert.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>Task representing the operation.</returns>
    Task InsertConsumersAsync(OutboxEnvelopeConsumerEntity[] consumers, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the given <paramref name="consumers"/>.
    /// </summary>
    /// <param name="consumers">Consumers to update.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>Task representing the operation.</returns>
    Task UpdateConsumersAsync(OutboxEnvelopeConsumerEntity[] consumers, CancellationToken cancellationToken);
}