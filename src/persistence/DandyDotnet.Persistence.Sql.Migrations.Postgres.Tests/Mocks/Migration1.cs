using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;

internal sealed class Migration1 : IMigration
{
    public long Version => 1;

    public void Up(IMigrationBuilder builder)
    {
    }

    public void Down(IMigrationBuilder builder)
    {
    }
}