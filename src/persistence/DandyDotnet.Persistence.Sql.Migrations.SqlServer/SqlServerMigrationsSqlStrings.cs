namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

public sealed class SqlServerMigrationsSqlStrings : MigrationsSqlStrings
{
    public override string MigrationsTableExists { get; }
    public override string CreateMigrationsTable { get; }
    public override string InsertMigrations { get; }
    public override string DeleteMigrations { get; }
}