namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

/// <summary>
///     Represents a database migration that can be applied or rolled back.
/// </summary>
/// <remarks>
///     <para>
///         Implement this interface to create custom database migrations. Each migration defines a specific version
///         and provides the logic to apply (<see cref="Up" />) and roll back (<see cref="Down" />) the migration.
///     </para>
///     <para>
///         The <see cref="Version" /> property uniquely identifies the migration and determines the order in which
///         migrations are applied. Migrations are executed in ascending order of their version numbers.
///     </para>
///     <para>
///         The <see cref="Up" /> method contains the SQL statements or database operations needed to apply the migration,
///         while the <see cref="Down" /> method contains the operations to revert the migration to its previous state.
///     </para>
/// </remarks>
public interface IMigration
{
    /// <summary>
    ///     Gets the unique version identifier for this migration.
    /// </summary>
    /// <returns>A long integer representing the migration version. Must be unique across all migrations.</returns>
    /// <remarks>
    ///     <para>
    ///         Migration versions determine the order in which migrations are applied. Lower version numbers are applied first.
    ///     </para>
    ///     <para>
    ///         It is recommended to use sequential version numbers (e.g., 1, 2, 3, ...) or timestamp-based versions
    ///         (e.g., 20240101000000) to ensure proper ordering.
    ///     </para>
    /// </remarks>
    long Version { get; }

    /// <summary>
    ///     Applies this migration to the database.
    /// </summary>
    /// <param name="builder">The migration builder used to execute SQL commands against the database.</param>
    /// <remarks>
    ///     <para>
    ///         Implement this method to define the database changes that this migration should apply.
    ///         Use the provided <paramref name="builder" /> to execute SQL commands.
    ///     </para>
    ///     <para>
    ///         This method is called during the <see cref="IMigrationRunner.MigrateUp" /> operation when the migration
    ///         has not yet been applied to the database.
    ///     </para>
    /// </remarks>
    /// <seealso cref="MigrationBuilderExtensions.Execute" />
    void Up(IMigrationBuilder builder);

    /// <summary>
    ///     Rolls back this migration from the database.
    /// </summary>
    /// <param name="builder">The migration builder used to execute SQL commands against the database.</param>
    /// <remarks>
    ///     <para>
    ///         Implement this method to define how to revert the database changes made by this migration.
    ///         Use the provided <paramref name="builder" /> to execute SQL commands.
    ///     </para>
    ///     <para>
    ///         This method is called during the <see cref="IMigrationRunner.MigrateDown" /> operation when rolling back
    ///         to a specific version or when the migration needs to be reverted.
    ///     </para>
    /// </remarks>
    /// <seealso cref="MigrationBuilderExtensions.Execute" />
    void Down(IMigrationBuilder builder);
}