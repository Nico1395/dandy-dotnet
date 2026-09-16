using System.Data;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
///     Defines a read-only context contract providing access to the active database connection and transaction.
/// </summary>
public interface IReadOnlyUnitOfWorkContext
{
    /// <summary>
    ///     Gets the active database connection.
    /// </summary>
    IDbConnection Connection { get; }

    /// <summary>
    ///     Gets the active database transaction.
    /// </summary>
    IDbTransaction Transaction { get; }

    /// <summary>
    ///     Gets a value indicating whether the unit of work transaction has been completed and committed.
    /// </summary>
    bool Completed { get; }
}