using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;

internal sealed class FailingMigration : IMigration
{
    public long Version => 3;

    public void Up(IMigrationBuilder builder)
    {
        MigrationExecutionRecorder.RecordUp(Version);
        throw new InvalidOperationException("Migration failed.");
    }

    public void Down(IMigrationBuilder builder)
    {
    }
}
