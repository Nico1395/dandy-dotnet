namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

public sealed class SqliteMigrationsSqlStrings : MigrationsSqlStrings
{
    private readonly MigrationsConfiguration _configuration;
    private readonly string _qualifiedTable;

    public SqliteMigrationsSqlStrings(MigrationsConfiguration configuration)
    {
        _configuration = configuration;
        _qualifiedTable = QuoteIdentifier(configuration.Table);
    }

    public override string GetAppliedVersions =>
        $"SELECT {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} FROM {_qualifiedTable} ORDER BY {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)};";

    public override string SchemaExists => "SELECT 1;";

    public override string CreateSchema => "SELECT 1;";

    public override string MigrationsTableExists =>
        $"SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{EscapeLiteral(_configuration.Table)}';";

    public override string CreateMigrationsTable =>
        $"""
        CREATE TABLE {_qualifiedTable} (
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} INTEGER NOT NULL PRIMARY KEY,
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)} DATETIME NOT NULL
        );
        """;

    public override string InsertMigrations =>
        $"INSERT INTO {_qualifiedTable} ({QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)}, {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)}) VALUES (@Version, CURRENT_TIMESTAMP);";

    public override string DeleteMigrations =>
        $"DELETE FROM {_qualifiedTable} WHERE {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} = @Version;";

    private static string QuoteIdentifier(string identifier) => $"\"{identifier.Replace("\"", "\"\"")}\"";

    private static string EscapeLiteral(string value) => value.Replace("'", "''");
}