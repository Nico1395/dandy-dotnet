using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Repositories;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
///     Represents a unit of work managing database transactions and repositories for SQL-based event persistence.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly UnitOfWorkContext _context;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
    /// </summary>
    /// <param name="dbConnectionFactory">The database connection factory resolved by service key.</param>
    /// <param name="sqlStrings">The provider of SQL command strings.</param>
    public UnitOfWork(
        [FromKeyedServices(EventSourcingConstants.ServiceKey)] IDbConnectionFactory dbConnectionFactory,
        SqlStrings sqlStrings)
    {
        _context = new UnitOfWorkContext(dbConnectionFactory);

        Envelopes = new EnvelopeRepository(sqlStrings, _context);
        Snapshots = new SnapshotRepository(sqlStrings, _context);
        Outbox = new OutboxRepository(sqlStrings, _context);
    }

    /// <inheritdoc />
    public IEnvelopeRepository Envelopes { get; }

    /// <inheritdoc />
    public ISnapshotRepository Snapshots { get; }

    /// <inheritdoc />
    public IOutboxRepository Outbox { get; }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _context.Commit(cancellationToken);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _context.Dispose();
    }
}