namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

public sealed class PostgresMigrationsSqlStrings : MigrationsSqlStrings
{
    private readonly MigrationsConfiguration _configuration;
    private readonly string _schema;
    private readonly string _qualifiedTable;

    public PostgresMigrationsSqlStrings(MigrationsConfiguration configuration)
    {
        _configuration = configuration;
        _schema = configuration.Schema ?? "public";
        _qualifiedTable = $"{QuoteIdentifier(_schema)}.{QuoteIdentifier(configuration.Table)}";
    }

    public override string GetAppliedVersions =>
        $"SELECT {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} FROM {_qualifiedTable} ORDER BY {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)};";

    public override string SchemaExists =>
        $"SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = '{EscapeLiteral(_schema)}';";

    public override string CreateSchema =>
        $"CREATE SCHEMA IF NOT EXISTS {QuoteIdentifier(_schema)};";

    public override string MigrationsTableExists =>
        $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{EscapeLiteral(_schema)}' AND table_name = '{EscapeLiteral(_configuration.Table)}';";

    public override string CreateMigrationsTable =>
        $"""
        CREATE TABLE {_qualifiedTable} (
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} BIGINT NOT NULL PRIMARY KEY,
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)} TIMESTAMPTZ NOT NULL
        );
        """;

    public override string InsertMigrations =>
        $"INSERT INTO {_qualifiedTable} ({QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)}, {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)}) VALUES (@Version, CURRENT_TIMESTAMP);";

    public override string DeleteMigrations =>
        $"DELETE FROM {_qualifiedTable} WHERE {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} = @Version;";

    private static string QuoteIdentifier(string identifier) => $"\"{identifier.Replace("\"", "\"\"")}\"";

    private static string EscapeLiteral(string value) => value.Replace("'", "''");
}