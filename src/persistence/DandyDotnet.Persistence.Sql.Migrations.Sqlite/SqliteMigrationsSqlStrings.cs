namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

public sealed class SqliteMigrationsSqlStrings : MigrationsSqlStrings
{
    public override string MigrationsTableExists { get; }
    public override string CreateMigrationsTable { get; }
    public override string InsertMigrations { get; }
    public override string DeleteMigrations { get; }
}