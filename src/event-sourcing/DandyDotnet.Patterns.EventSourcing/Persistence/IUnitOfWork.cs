namespace DandyDotnet.Patterns.EventSourcing.Persistence;

public interface IUnitOfWork
{
    IEnvelopeRepository Envelopes { get; }
    ISnapshotRepository Snapshots { get; }
    IOutboxRepository Outbox { get; }
    IProjectionRepository Projections { get; }
    Task CommitAsync(CancellationToken cancellationToken);
}