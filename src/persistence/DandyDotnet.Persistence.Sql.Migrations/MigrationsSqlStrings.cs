namespace DandyDotnet.Persistence.Sql.Migrations;

public abstract class MigrationsSqlStrings
{
    public abstract string SchemaExists { get; } 
    public abstract string CreateSchema { get; } 
    public abstract string MigrationsTableExists { get; }
    public abstract string CreateMigrationsTable { get; }
    public abstract string InsertMigrations { get; }
    public abstract string DeleteMigrations { get; }
}