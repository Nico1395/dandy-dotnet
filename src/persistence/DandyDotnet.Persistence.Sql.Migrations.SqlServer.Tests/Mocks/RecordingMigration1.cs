using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests.Mocks;

internal sealed class RecordingMigration1 : IMigration
{
    public long Version => 1;

    public void Up(IMigrationBuilder builder)
    {
        MigrationExecutionRecorder.RecordUp(Version);
    }

    public void Down(IMigrationBuilder builder)
    {
        MigrationExecutionRecorder.RecordDown(Version);
    }
}
