namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

public interface IMigrationRunner
{
    void MigrateUp();
    void MigrateDown(long? toVersion);
}