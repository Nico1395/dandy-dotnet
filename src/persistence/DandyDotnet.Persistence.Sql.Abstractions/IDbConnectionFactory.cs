using System.Data;

namespace DandyDotnet.Persistence.Sql.Abstractions;

/// <summary>
///     Represents a factory for creating <see cref="IDbConnection" /> instances.
/// </summary>
/// <remarks>
///     <para>
///         This interface provides a simple abstraction for creating database connections without tying consuming code
///         to a specific database provider.
///     </para>
///     <para>
///         Implementations should handle connection string management and create appropriate connection instances
///         for their target database system (e.g., SQL Server, PostgreSQL, SQLite).
///     </para>
/// </remarks>
public interface IDbConnectionFactory
{
    /// <summary>
    ///     Creates a new database connection instance.
    /// </summary>
    /// <returns>
    ///     A new <see cref="IDbConnection" /> that can be opened and used to execute commands against a database.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         The returned connection is in a closed state and must be explicitly opened before use.
    ///         Callers are responsible for disposing of the connection when it is no longer needed.
    ///     </para>
    /// </remarks>
    IDbConnection Create();
}