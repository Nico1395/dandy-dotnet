using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;

internal sealed class RecordingMigration2 : IMigration
{
    public long Version => 2;

    public void Up(IMigrationBuilder builder)
    {
        MigrationExecutionRecorder.Record(Version);
    }

    public void Down(IMigrationBuilder builder)
    {
    }
}
