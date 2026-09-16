using System.Data;

namespace DandyDotnet.Persistence.Sql.Abstractions;

/// <summary>
///     Extension methods for <see cref="IDbConnectionFactory" /> that provide convenience operations.
/// </summary>
public static class DbConnectionFactoryExtensions
{
    /// <summary>
    ///     Creates and opens a new database connection in a single operation.
    /// </summary>
    /// <param name="factory">The database connection factory to use for creating the connection.</param>
    /// <returns>
    ///     An opened <see cref="IDbConnection" /> ready for use.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="factory" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The connection could not be opened (e.g., invalid connection string, network issues, authentication failures).
    /// </exception>
    /// <exception cref="System.Data.Common.DbException">
    ///     A database-specific error occurred while opening the connection.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method combines the <see cref="IDbConnectionFactory.Create" /> and <see cref="IDbConnection.Open" /> operations
    ///         into a single call, which is useful for reducing boilerplate code.
    ///     </para>
    ///     <para>
    ///         Callers are still responsible for disposing of the connection when it is no longer needed.
    ///         Consider using this method within a <c>using</c> statement or ensuring proper disposal through other means.
    ///     </para>
    /// </remarks>
    public static IDbConnection CreateAndOpen(this IDbConnectionFactory factory)
    {
        var connection = factory.Create();
        connection.Open();
        return connection;
    }
}