namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

/// <summary>
///     PostgreSQL-specific implementation of <see cref="MigrationsSqlStrings" />.
/// </summary>
/// <remarks>
///     <para>
///         This class provides PostgreSQL-specific SQL strings for the migrations framework.
///         It includes the appropriate SQL syntax for PostgreSQL databases.
///     </para>
/// </remarks>
/// <seealso cref="MigrationsSqlStrings" />

public sealed class PostgresMigrationsSqlStrings : MigrationsSqlStrings
{
    private readonly MigrationsConfiguration _configuration;
    private readonly string _schema;
    private readonly string _qualifiedTable;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PostgresMigrationsSqlStrings" /> class.
    /// </summary>
    /// <param name="configuration">The migrations configuration.</param>
    /// <remarks>
    ///     <para>
    ///         The constructor validates the configuration and initializes the schema and qualified table names.
    ///     </para>
    ///     <para>
    ///         If <see cref="MigrationsConfiguration.Schema" /> is not specified, the default "public" schema is used.
    ///     </para>
    /// </remarks>

    public PostgresMigrationsSqlStrings(MigrationsConfiguration configuration)
    {
        _configuration = configuration;
        _schema = configuration.Schema ?? "public";
        _qualifiedTable = $"{QuoteIdentifier(_schema)}.{QuoteIdentifier(configuration.Table)}";
    }

    /// <summary>
    ///     Gets the SQL string to retrieve all applied migration versions from the migrations table.
    /// </summary>
    /// <value>A PostgreSQL query string that returns the version numbers of all applied migrations.</value>

    public override string GetAppliedVersions =>
        $"SELECT {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} FROM {_qualifiedTable} ORDER BY {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)};";

    /// <summary>
    ///     Gets the SQL string to check if the database schema exists.
    /// </summary>
    /// <value>A PostgreSQL query string that returns a count indicating if the schema exists.</value>

    public override string SchemaExists =>
        $"SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = '{EscapeLiteral(_schema)}';";

    /// <summary>
    ///     Gets the SQL string to create the database schema if it does not exist.
    /// </summary>
    /// <value>A PostgreSQL command string that creates the schema.</value>

    public override string CreateSchema =>
        $"CREATE SCHEMA IF NOT EXISTS {QuoteIdentifier(_schema)};";

    /// <summary>
    ///     Gets the SQL string to check if the migrations table exists in the database.
    /// </summary>
    /// <value>A PostgreSQL query string that returns a count indicating if the table exists.</value>

    public override string MigrationsTableExists =>
        $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{EscapeLiteral(_schema)}' AND table_name = '{EscapeLiteral(_configuration.Table)}';";

    /// <summary>
    ///     Gets the SQL string to create the migrations tracking table.
    /// </summary>
    /// <value>A PostgreSQL command string that creates the migrations table with the appropriate structure.</value>

    public override string CreateMigrationsTable =>
        $"""
        CREATE TABLE {_qualifiedTable} (
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} BIGINT NOT NULL PRIMARY KEY,
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)} TIMESTAMPTZ NOT NULL
        );
        """;

    /// <summary>
    ///     Gets the SQL string to insert records for applied migrations into the migrations table.
    /// </summary>
    /// <value>A PostgreSQL command string that inserts migration records with parameter placeholders.</value>

    public override string InsertMigrations =>
        $"INSERT INTO {_qualifiedTable} ({QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)}, {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)}) VALUES (@Version, CURRENT_TIMESTAMP);";

    /// <summary>
    ///     Gets the SQL string to delete migration records from the migrations table.
    /// </summary>
    /// <value>A PostgreSQL command string that deletes migration records with parameter placeholders.</value>

    public override string DeleteMigrations =>
        $"DELETE FROM {_qualifiedTable} WHERE {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} = @Version;";

    /// <summary>
    ///     Quotes a PostgreSQL identifier to prevent SQL injection and handle special characters.
    /// </summary>
    /// <param name="identifier">The identifier to quote.</param>
    /// <returns>The quoted identifier as a string.</returns>

    private static string QuoteIdentifier(string identifier) => $"\"{identifier.Replace("\"", "\"\"")}\"";

    /// <summary>
    ///     Escapes literal values for use in PostgreSQL SQL strings to prevent SQL injection.
    /// </summary>
    /// <param name="value">The value to escape.</param>
    /// <returns>The escaped value as a string.</returns>

    private static string EscapeLiteral(string value) => value.Replace("'", "''");
}