using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Mocks;

internal sealed class FailingDownMigration : IMigration
{
    public long Version => 2;

    public void Up(IMigrationBuilder builder)
    {
        MigrationExecutionRecorder.RecordUp(Version);
    }

    public void Down(IMigrationBuilder builder)
    {
        MigrationExecutionRecorder.RecordDown(Version);
        throw new InvalidOperationException("Migration down failed.");
    }
}
