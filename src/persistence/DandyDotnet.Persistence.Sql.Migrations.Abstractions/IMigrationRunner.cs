namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

/// <summary>
///     Provides functionality to manage and execute database migrations.
/// </summary>
/// <remarks>
///     <para>
///         This interface defines the core operations for database migration management, including
///         initializing the database schema, checking for applied migrations, applying pending migrations,
///         and rolling back migrations.
///     </para>
///     <para>
///         The migration runner maintains a record of applied migrations in a dedicated database table
///         and uses this information to determine which migrations need to be applied or rolled back.
///     </para>
/// </remarks>
public interface IMigrationRunner
{
    /// <summary>
    ///     Gets an array of version numbers for all migrations that have been applied to the database.
    /// </summary>
    /// <returns>An array of long integers representing the versions of applied migrations, ordered by version.</returns>
    /// <remarks>
    ///     <para>
    ///         This method queries the migrations table in the database and returns the versions of all
    ///         migrations that have been successfully applied. If the migrations table does not exist,
    ///         it will be created automatically.
    ///     </para>
    ///     <para>
    ///         The returned array is ordered in ascending order by version number.
    ///     </para>
    /// </remarks>
    long[] GetAppliedVersions();

    /// <summary>
    ///     Determines whether there are any migrations that have not been applied to the database.
    /// </summary>
    /// <returns><see langword="true" /> if there are unapplied migrations; otherwise, <see langword="false" />.</returns>
    /// <remarks>
    ///         This method compares the list of registered migrations with the list of applied migrations
    ///         to determine if any migrations remain to be applied.
    ///     </remarks>
    bool HasUnappliedMigrations();

    /// <summary>
    ///     Initializes the database schema and migrations table.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method ensures that the required database schema and migrations table exist.
    ///         If the schema does not exist, it will be created. If the migrations table does not exist,
    ///         it will be created with the appropriate structure.
    ///     </para>
    ///     <para>
    ///         This method is automatically called by <see cref="MigrateUp" /> and <see cref="MigrateDown" />
    ///         before performing any migration operations.
    ///     </para>
    /// </remarks>
    void InitializeDatabase();

    /// <summary>
    ///     Applies all pending migrations to the database in ascending order of their version numbers.
    /// </summary>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during migration execution. The exception contains details about
    ///     the specific migration that failed.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method applies all migrations that have not yet been applied to the database.
    ///         Migrations are executed in ascending order of their version numbers.
    ///     </para>
    ///     <para>
    ///         The method executes within a transaction, so if any migration fails, all applied migrations
    ///         in the current batch will be rolled back.
    ///     </para>
    ///     <para>
    ///         After each successful migration, a record is inserted into the migrations table to track
    ///         that the migration has been applied.
    ///     </para>
    /// </remarks>
    void MigrateUp();

    /// <summary>
    ///     Rolls back migrations from the database to the specified version.
    /// </summary>
    /// <param name="toVersion">
    ///     The target version to roll back to, or <see langword="null" /> to roll back all applied migrations.
    /// </param>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during rollback execution. The exception contains details about
    ///     the specific migration that failed to roll back.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method rolls back migrations in descending order of their version numbers, starting
    ///         from the highest applied version down to the specified <paramref name="toVersion" />.
    ///     </para>
    ///     <para>
    ///         If <paramref name="toVersion" /> is <see langword="null" />, all applied migrations will be rolled back.
    ///         If <paramref name="toVersion" /> is specified, only migrations with version greater than or equal to
    ///         the specified version will be rolled back.
    ///     </para>
    ///     <para>
    ///         The method executes within a transaction, so if any rollback fails, all rolled back migrations
    ///         in the current batch will be reverted.
    ///     </para>
    ///     <para>
    ///         After each successful rollback, the corresponding record is removed from the migrations table.
    ///     </para>
    /// </remarks>
    void MigrateDown(long? toVersion);
}