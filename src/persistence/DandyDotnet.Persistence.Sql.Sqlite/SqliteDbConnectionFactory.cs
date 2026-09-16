using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Persistence.Sql.Sqlite;

/// <summary>
///     A factory for creating SQLite database connections using Microsoft.Data.Sqlite.
/// </summary>
/// <param name="connectionString">
///     The connection string used to connect to the SQLite database.
///     This is typically a file path (e.g., <c>Data Source=myDatabase.db</c>) or <c>:memory:</c> for an in-memory database.
/// </param>
/// <remarks>
///     <para>
///         This factory creates <see cref="SqliteConnection" /> instances configured with the provided connection string.
///         It is designed for use with SQLite databases and relies on the Microsoft.Data.Sqlite ADO.NET provider.
///     </para>
///     <para>
///         The connection string is not validated during construction. It will be validated when <see cref="Create" />
///         is first called. This allows for deferred validation and supports scenarios where the connection string
///         might not be available at construction time.
///     </para>
///     <para>
///         For SQLite, the connection string commonly uses the <c>Data Source</c> parameter to specify the database file path.
///         Other parameters such as <c>Mode</c>, <c>Cache</c>, and <c>Password</c> (for encrypted databases) can also be used.
///     </para>
/// </remarks>
public sealed class SqliteDbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    /// <summary>
    ///     Creates a new database connection instance.
    /// </summary>
    /// <returns>
    ///     A new <see cref="SqliteConnection" /> configured with the provided connection string.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <c>connectionString</c> is <see langword="null" /> or whitespace.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         The returned connection is in a closed state and must be explicitly opened before use.
    ///         Callers are responsible for disposing the connection when it is no longer needed.
    ///     </para>
    /// </remarks>
    public IDbConnection Create()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new SqliteConnection(connectionString);
    }
}