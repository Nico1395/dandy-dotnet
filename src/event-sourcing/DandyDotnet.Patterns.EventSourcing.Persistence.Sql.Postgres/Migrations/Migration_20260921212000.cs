using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres.Migrations;

internal sealed class Migration_20260921212000 : IMigration
{
    public long Version => 20260921212000;

    public void Up(IMigrationBuilder builder)
    {
        builder.Execute($"""
                         ALTER TABLE "{Sql.Constants.Schema.Name}"."{Tables.Envelopes.Table}"
                         ADD COLUMN "{Tables.Envelopes.Tags}" TEXT NULL
                         """);
    }

    public void Down(IMigrationBuilder builder)
    {
        builder.Execute($"""
                         ALTER TABLE "{Sql.Constants.Schema.Name}"."{Tables.Envelopes.Table}"
                         DROP COLUMN "{Tables.Envelopes.Tags}"
                         """);
    }
}