namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

public interface IMigrationRunner
{
    long[] GetAppliedVersions();
    bool HasUnappliedMigrations();
    void InitializeDatabase();
    void MigrateUp();
    void MigrateDown(long? toVersion);
}