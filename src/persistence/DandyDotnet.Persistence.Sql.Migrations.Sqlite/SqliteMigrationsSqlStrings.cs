namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

/// <summary>
///     SQLite-specific implementation of <see cref="MigrationsSqlStrings" />.
/// </summary>
/// <remarks>
///     <para>
///         This class provides SQLite-specific SQL strings for the migrations framework.
///         It includes the appropriate SQL syntax for SQLite databases.
///     </para>
/// </remarks>
/// <seealso cref="MigrationsSqlStrings" />

public sealed class SqliteMigrationsSqlStrings : MigrationsSqlStrings
{
    private readonly MigrationsConfiguration _configuration;
    private readonly string _qualifiedTable;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SqliteMigrationsSqlStrings" /> class.
    /// </summary>
    /// <param name="configuration">The migrations configuration.</param>
    /// <remarks>
    ///     <para>
    ///         The constructor validates the configuration and initializes the qualified table name.
    ///     </para>
    ///     <para>
    ///         SQLite does not support schemas, so only the table name is used.
    ///     </para>
    /// </remarks>

    public SqliteMigrationsSqlStrings(MigrationsConfiguration configuration)
    {
        _configuration = configuration;
        _qualifiedTable = QuoteIdentifier(configuration.Table);
    }

    /// <summary>
    ///     Gets the SQL string to retrieve all applied migration versions from the migrations table.
    /// </summary>
    /// <value>An SQLite query string that returns the version numbers of all applied migrations.</value>

    public override string GetAppliedVersions =>
        $"SELECT {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} FROM {_qualifiedTable} ORDER BY {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)};";

    /// <summary>
    ///     Gets the SQL string to check if the database schema exists.
    /// </summary>
    /// <value>An SQLite query string that always returns true (SQLite does not use schemas).</value>

    public override string SchemaExists => "SELECT 1;";

    /// <summary>
    ///     Gets the SQL string to create the database schema if it does not exist.
    /// </summary>
    /// <value>An SQLite command that does nothing (SQLite does not use schemas).</value>

    public override string CreateSchema => "SELECT 1;";

    /// <summary>
    ///     Gets the SQL string to check if the migrations table exists in the database.
    /// </summary>
    /// <value>An SQLite query string that returns a count indicating if the table exists.</value>

    public override string MigrationsTableExists =>
        $"SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{EscapeLiteral(_configuration.Table)}';";

    /// <summary>
    ///     Gets the SQL string to create the migrations tracking table.
    /// </summary>
    /// <value>An SQLite command string that creates the migrations table with the appropriate structure.</value>

    public override string CreateMigrationsTable =>
        $"""
        CREATE TABLE {_qualifiedTable} (
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} INTEGER NOT NULL PRIMARY KEY,
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)} DATETIME NOT NULL
        );
        """;

    /// <summary>
    ///     Gets the SQL string to insert records for applied migrations into the migrations table.
    /// </summary>
    /// <value>An SQLite command string that inserts migration records with parameter placeholders.</value>

    public override string InsertMigrations =>
        $"INSERT INTO {_qualifiedTable} ({QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)}, {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)}) VALUES (@Version, CURRENT_TIMESTAMP);";

    /// <summary>
    ///     Gets the SQL string to delete migration records from the migrations table.
    /// </summary>
    /// <value>An SQLite command string that deletes migration records with parameter placeholders.</value>

    public override string DeleteMigrations =>
        $"DELETE FROM {_qualifiedTable} WHERE {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} = @Version;";

    /// <summary>
    ///     Quotes an SQLite identifier to prevent SQL injection and handle special characters.
    /// </summary>
    /// <param name="identifier">The identifier to quote.</param>
    /// <returns>The quoted identifier as a string.</returns>

    private static string QuoteIdentifier(string identifier) => $"\"{identifier.Replace("\"", "\"\"")}\"";

    /// <summary>
    ///     Escapes literal values for use in SQLite SQL strings to prevent SQL injection.
    /// </summary>
    /// <param name="value">The value to escape.</param>
    /// <returns>The escaped value as a string.</returns>

    private static string EscapeLiteral(string value) => value.Replace("'", "''");
}