using System.Data;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Implementation of <see cref="IMigrationBuilder" /> that provides database connection and transaction
///     for executing migration operations.
/// </summary>
/// <remarks>
///     <para>
///         This class is created by the <see cref="MigrationRunner" /> and passed to the <see cref="IMigration.Up" />
///         and <see cref="IMigration.Down" /> methods of each migration during execution.
///     </para>
///     <para>
///         The class implements <see cref="IMigrationBuilder" /> and provides access to an open database connection
///         and an active transaction for migration operations.
///     </para>
/// </remarks>
internal sealed class MigrationBuilder(IDbConnection connection, IDbTransaction transaction) : IMigrationBuilder
{
    /// <summary>
    ///     Gets the database connection for migration operations.
    /// </summary>
    /// <returns>The <see cref="IDbConnection" /> instance provided during construction.</returns>
    public IDbConnection Connection => connection;

    /// <summary>
    ///     Gets the database transaction for migration operations.
    /// </summary>
    /// <returns>The <see cref="IDbTransaction" /> instance provided during construction.</returns>
    public IDbTransaction Transaction => transaction;
}