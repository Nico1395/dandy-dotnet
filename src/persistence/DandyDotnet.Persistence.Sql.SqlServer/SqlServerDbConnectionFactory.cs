using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Data.SqlClient;

namespace DandyDotnet.Persistence.Sql.SqlServer;

/// <summary>
///     A factory for creating SQL Server database connections using Microsoft.Data.SqlClient.
/// </summary>
/// <param name="connectionString">
///     The connection string used to connect to the SQL Server database.
///     This should be a valid SQL Server connection string containing the necessary parameters such as Server, Database,
///     User Id, Password, and optionally Trusted_Connection,Encrypt, etc.
/// </param>
/// <remarks>
///     <para>
///         This factory creates <see cref="SqlConnection" /> instances configured with the provided connection string.
///         It is designed for use with Microsoft SQL Server databases and relies on the Microsoft.Data.SqlClient ADO.NET provider.
///     </para>
///     <para>
///         The connection string is not validated during construction. It will be validated when <see cref="Create" />
///         is first called. This allows for deferred validation and supports scenarios where the connection string
///         might not be available at construction time.
///     </para>
///     <para>
///         For SQL Server, the connection string commonly uses parameters such as <c>Server</c> or <c>Data Source</c> for the
///         server name, <c>Initial Catalog</c> or <c>Database</c> for the database name, and authentication parameters
///         like <c>User Id</c>, <c>Password</c>, or <c>Trusted_Connection</c>.
///     </para>
/// </remarks>
public sealed class SqlServerDbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    /// <summary>
    ///     Creates a new database connection instance.
    /// </summary>
    /// <returns>
    ///     A new <see cref="SqlConnection" /> configured with the provided connection string.
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
        return new SqlConnection(connectionString);
    }
}