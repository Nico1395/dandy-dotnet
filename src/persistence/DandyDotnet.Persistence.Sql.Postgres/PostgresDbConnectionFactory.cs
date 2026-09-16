using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Npgsql;

namespace DandyDotnet.Persistence.Sql.Postgres;

/// <summary>
///     A factory for creating PostgreSQL database connections using Npgsql.
/// </summary>
/// <param name="connectionString">
///     The connection string used to connect to the PostgreSQL database.
///     This should be a valid Npgsql connection string containing the necessary parameters such as Host, Database,
///     Username, and Password.
/// </param>
/// <remarks>
///     <para>
///         This factory creates <see cref="NpgsqlConnection" /> instances configured with the provided connection string.
///         It is designed for use with PostgreSQL databases and relies on the Npgsql ADO.NET provider.
///     </para>
///     <para>
///         The connection string is not validated during construction. It will be validated when <see cref="Create" />
///         is first called. This allows for deferred validation and supports scenarios where the connection string
///         might not be available at construction time.
///     </para>
/// </remarks>
public sealed class PostgresDbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    /// <summary>
    ///     Creates a new database connection instance.
    /// </summary>
    /// <returns>
    ///     A new <see cref="NpgsqlConnection" /> configured with the provided connection string.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <c>connectionString</c> is <see langword="null" /> or whitespace.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         The returned connection is in a closed state and must be explicitly opened before use.
    ///         Callers are responsible for disposing of the connection when it is no longer needed.
    ///     </para>
    /// </remarks>
    public IDbConnection Create()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new NpgsqlConnection(connectionString);
    }
}