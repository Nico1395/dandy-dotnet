namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Abstract base class that provides SQL strings for database-specific migration operations.
/// </summary>
/// <remarks>
///     <para>
///         Each database provider (PostgreSQL, SQL Server, SQLite) implements this class to provide
///         the appropriate SQL syntax for that database system.
///     </para>
///     <para>
///         The SQL strings are used by the <see cref="MigrationRunner" /> to perform database operations
///         such as checking for applied migrations, creating the migrations table, and inserting/deleting
///         migration records.
///     </para>
/// </remarks>
public abstract class MigrationsSqlStrings
{
    /// <summary>
    ///     Gets the SQL string to retrieve all applied migration versions from the migrations table.
    /// </summary>
    /// <returns>A SQL query string that returns the version numbers of all applied migrations.</returns>
    public abstract string GetAppliedVersions { get; }

    /// <summary>
    ///     Gets the SQL string to check if the database schema exists.
    /// </summary>
    /// <returns>A SQL query string that returns a count or boolean indicating if the schema exists.</returns>
    public abstract string SchemaExists { get; }

    /// <summary>
    ///     Gets the SQL string to create the database schema if it does not exist.
    /// </summary>
    /// <returns>A SQL command string that creates the schema.</returns>
    public abstract string CreateSchema { get; }

    /// <summary>
    ///     Gets the SQL string to check if the migrations table exists in the database.
    /// </summary>
    /// <returns>A SQL query string that returns a count or boolean indicating if the table exists.</returns>
    public abstract string MigrationsTableExists { get; }

    /// <summary>
    ///     Gets the SQL string to create the migrations tracking table.
    /// </summary>
    /// <returns>A SQL command string that creates the migrations table with the appropriate structure.</returns>
    /// <remarks>
    ///         The migrations table stores the version numbers of applied migrations and their application timestamps.
    ///     </remarks>
    public abstract string CreateMigrationsTable { get; }

    /// <summary>
    ///     Gets the SQL string to insert records for applied migrations into the migrations table.
    /// </summary>
    /// <returns>A SQL command string that inserts migration records, typically with parameter placeholders.</returns>
    public abstract string InsertMigrations { get; }

    /// <summary>
    ///     Gets the SQL string to delete migration records from the migrations table.
    /// </summary>
    /// <returns>A SQL command string that deletes migration records, typically with parameter placeholders.</returns>
    public abstract string DeleteMigrations { get; }
}