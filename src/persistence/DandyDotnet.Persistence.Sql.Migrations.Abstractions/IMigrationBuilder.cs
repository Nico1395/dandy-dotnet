using System.Data;

namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

/// <summary>
///     Provides the necessary database connection and transaction for executing migration operations.
/// </summary>
/// <remarks>
///     <para>
///         This interface is implemented by the migration framework and passed to the <see cref="IMigration.Up" /> and
///         <see cref="IMigration.Down" /> methods of each migration.
///     </para>
///     <para>
///         The <see cref="Connection" /> property provides access to the database connection, while the
///         <see cref="Transaction" /> property provides access to the current database transaction.
///     </para>
///     <para>
///         All migration operations should be executed within the context of the provided transaction to ensure
///         atomicity. If any migration fails, the entire transaction will be rolled back.
///     </para>
/// </remarks>
/// <seealso cref="IMigration.Up" />
/// <seealso cref="IMigration.Down" />
/// <seealso cref="MigrationBuilderExtensions.Execute" />
public interface IMigrationBuilder
{
    /// <summary>
    ///     Gets the database connection used for migration operations.
    /// </summary>
    /// <returns>An open <see cref="IDbConnection" /> to the database.</returns>
    /// <remarks>
    ///         The connection is opened and managed by the migration runner. Do not close or dispose this connection.
    ///     </remarks>
    public IDbConnection Connection { get; }

    /// <summary>
    ///     Gets the database transaction used for migration operations.
    /// </summary>
    /// <returns>An active <see cref="IDbTransaction" /> for the current migration operation.</returns>
    /// <remarks>
    ///     <para>
    ///         All migration operations are executed within the context of this transaction. If any error occurs
    ///         during migration execution, the transaction will be rolled back automatically.
    ///     </para>
    ///     <para>
    ///         Do not commit or roll back this transaction manually. The migration runner handles transaction lifecycle.
    ///     </para>
    /// </remarks>
    public IDbTransaction Transaction { get; }
}