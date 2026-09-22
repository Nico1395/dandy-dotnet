using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

namespace DandyDotnet.Patterns.EventSourcing.Persistence;

public interface IEnvelopeRepository
{
    Task<long> GetStreamVersionAsync(string streamId, CancellationToken cancellationToken);
    Task<EnvelopeEntity[]> GetEnvelopesAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken);
    Task<EnvelopeEntity[]> GetEnvelopesAsync(string[] tags, CancellationToken cancellationToken);
    Task InsertAsync(string streamId, EnvelopeEntity[] envelopes, CancellationToken cancellationToken);
}