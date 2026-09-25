using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

namespace DandyDotnet.Patterns.EventSourcing.Persistence;

/// <summary>
/// Repository pattern for envelop-related database operations.
/// </summary>
public interface IEnvelopeRepository
{
    /// <summary>
    /// Gets the current stream version of the stream with the <paramref name="streamId"/>.
    /// </summary>
    /// <param name="streamId">ID of the stream to get the version for.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>The stream's version. Returns <c>0</c> if the stream does not exist yet.</returns>
    Task<long> GetStreamVersionAsync(string streamId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets envelopes of the stream with the <paramref name="streamId"/> with the filter parameters applied.
    /// </summary>
    /// <param name="streamId">ID of the target event stream.</param>
    /// <param name="fromVersion">Minimum version.</param>
    /// <param name="toVersion">Maximum version.</param>
    /// <param name="fromTimestamp">Minimum timestamp.</param>
    /// <param name="toTimestamp">Maximum timestamp.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>The envelopes that comply with the given conditions.</returns>
    Task<EnvelopeEntity[]> GetEnvelopesAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets envelopes that are associated with at least one of the given <paramref name="tags"/>.
    /// </summary>
    /// <param name="tags">Tags to query for.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>Envelopes that are associated with at least one of the given <paramref name="tags"/>.</returns>
    Task<EnvelopeEntity[]> GetEnvelopesAsync(string[] tags, CancellationToken cancellationToken);
    
    /// <summary>
    /// Inserts the given <paramref name="envelopes"/>.
    /// </summary>
    /// <param name="streamId">ID of the stream the envelopes belong to.</param>
    /// <param name="envelopes">Envelopes to insert.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>The envelopes that comply with the given conditions.</returns>
    Task InsertAsync(string streamId, EnvelopeEntity[] envelopes, CancellationToken cancellationToken);
}