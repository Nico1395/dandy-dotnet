namespace DandyDotnet.Patterns.EventSourcing.Persistence;

/// <summary>
/// Unit of work for transactional database operations.
/// </summary>
/// <remarks>
///     <para>
///         Is only intended for internal use and should not be used outside event sourcing.
///     </para>
///     <para>
///         Is being implemented by database drivers. Package <c>DandyDotnet.Patterns.EventSourcing.Persistence.Sql</c>
///         contains a base implementation for drivers of SQL database systems.
///     </para>
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Gets the repository for envelopes.
    /// </summary>
    IEnvelopeRepository Envelopes { get; }
    
    /// <summary>
    /// Gets the snapshot repository.
    /// </summary>
    ISnapshotRepository Snapshots { get; }
    
    /// <summary>
    /// Gets the outbox repository.
    /// </summary>
    IOutboxRepository Outbox { get; }
    
    /// <summary>
    /// Commits the transaction, closes the connection. Opens a new connection and transaction afterward.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for cancelling asynchronous operations.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the <paramref name="cancellationToken"/> was cancelled.</exception>
    Task CommitAsync(CancellationToken cancellationToken);
}