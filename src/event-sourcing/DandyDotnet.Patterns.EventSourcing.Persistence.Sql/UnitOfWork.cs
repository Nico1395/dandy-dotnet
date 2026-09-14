using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Repositories;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

internal sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly UnitOfWorkContext _context;

    public UnitOfWork(
        [FromKeyedServices(EventSourcingConstants.ServiceKey)] IDbConnectionFactory dbConnectionFactory,
        SqlStrings sqlStrings)
    {
        _context = new UnitOfWorkContext(dbConnectionFactory);

        Envelopes = new EnvelopeRepository(sqlStrings, _context);
        Snapshots = new SnapshotRepository(sqlStrings, _context);
        Outbox = new OutboxRepository(sqlStrings, _context);
    }

    public IEnvelopeRepository Envelopes { get; }
    public ISnapshotRepository Snapshots { get; }
    public IOutboxRepository Outbox { get; }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _context.Commit(cancellationToken);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}