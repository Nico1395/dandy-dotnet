namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

public sealed class SqlServerMigrationsSqlStrings : MigrationsSqlStrings
{
    private readonly MigrationsConfiguration _configuration;
    private readonly string _schema;
    private readonly string _qualifiedTable;

    public SqlServerMigrationsSqlStrings(MigrationsConfiguration configuration)
    {
        _configuration = configuration;
        _schema = configuration.Schema ?? "dbo";
        _qualifiedTable = $"{QuoteIdentifier(_schema)}.{QuoteIdentifier(configuration.Table)}";
    }

    public override string GetAppliedVersions =>
        $"SELECT {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} FROM {_qualifiedTable} ORDER BY {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)};";

    public override string SchemaExists =>
        $"SELECT COUNT(*) FROM sys.schemas WHERE name = N'{EscapeLiteral(_schema)}';";

    public override string CreateSchema =>
        $"IF SCHEMA_ID(N'{EscapeLiteral(_schema)}') IS NULL EXEC(N'CREATE SCHEMA {EscapeIdentifierForLiteral(_schema)}');";

    public override string MigrationsTableExists =>
        $"SELECT COUNT(*) FROM sys.tables t INNER JOIN sys.schemas s ON s.schema_id = t.schema_id WHERE s.name = N'{EscapeLiteral(_schema)}' AND t.name = N'{EscapeLiteral(_configuration.Table)}';";

    public override string CreateMigrationsTable =>
        $"""
        CREATE TABLE {_qualifiedTable} (
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} BIGINT NOT NULL PRIMARY KEY,
            {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)} DATETIME2 NOT NULL
        );
        """;

    public override string InsertMigrations =>
        $"INSERT INTO {_qualifiedTable} ({QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)}, {QuoteIdentifier(MigrationsConstants.Tables.Migrations.AppliedAt)}) VALUES (@Version, SYSUTCDATETIME());";

    public override string DeleteMigrations =>
        $"DELETE FROM {_qualifiedTable} WHERE {QuoteIdentifier(MigrationsConstants.Tables.Migrations.Version)} = @Version;";

    private static string QuoteIdentifier(string identifier) => $"[{identifier.Replace("]", "]]")}]";

    private static string EscapeIdentifierForLiteral(string identifier) =>
        QuoteIdentifier(identifier).Replace("'", "''");

    private static string EscapeLiteral(string value) => value.Replace("'", "''");
}