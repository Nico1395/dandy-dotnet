using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
///     Represents the runtime execution context managing database connections and active transactions for SQL persistence.
/// </summary>
internal sealed class UnitOfWorkContext : IReadOnlyUnitOfWorkContext, IDisposable
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private bool _disposed;
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="UnitOfWorkContext" /> class.
    /// </summary>
    /// <param name="dbConnectionFactory">The database connection factory.</param>
    public UnitOfWorkContext(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;

        Connection = dbConnectionFactory.CreateAndOpen();
        Transaction = Connection.BeginTransaction();
    }

    /// <inheritdoc />
    public IDbConnection Connection { get; private set; }

    /// <inheritdoc />
    public IDbTransaction Transaction { get; private set; }

    /// <inheritdoc />
    public bool Completed { get; private set; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;

        if (!Completed)
            Transaction.Rollback();

        Transaction.Dispose();
        Connection.Dispose();
        _disposed = true;
    }

    internal void Commit(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            Transaction.Commit();
            Completed = true;
        }
        catch
        {
            try
            {
                Transaction.Rollback();
            }
            finally
            {
                Transaction.Dispose();
                Connection.Dispose();
                _disposed = true;
            }

            throw;
        }

        Transaction.Dispose();
        Connection.Dispose();

        Connection = _dbConnectionFactory.CreateAndOpen();
        Transaction = Connection.BeginTransaction();
        Completed = false;
    }
}