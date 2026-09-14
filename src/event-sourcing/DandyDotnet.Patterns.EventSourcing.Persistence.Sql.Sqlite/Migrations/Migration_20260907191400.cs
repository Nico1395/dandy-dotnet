using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using FluentMigrator;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Migrations;

[Migration(20260907205400, "Creating tables")]
public class Migration_20260907205400 : Migration
{
    public override void Up()
    {
        Create.Table(Tables.Envelopes.Table)
            .WithColumn(Tables.Envelopes.StreamId).AsString(Tables.Envelopes.StreamIdLength).NotNullable().PrimaryKey()
            .WithColumn(Tables.Envelopes.Version).AsInt64().NotNullable().PrimaryKey()
            .WithColumn(Tables.Envelopes.Payload).AsString(Tables.Envelopes.PayloadLength).NotNullable()
            .WithColumn(Tables.Envelopes.Timestamp).AsDateTime().NotNullable()
            .WithColumn(Tables.Envelopes.EventKey).AsString(Tables.Envelopes.EventKeyLength).NotNullable();

        Create.Table(Tables.Snapshots.Table)
            .WithColumn(Tables.Snapshots.StreamId).AsString(Tables.Snapshots.StreamIdLength).NotNullable().PrimaryKey()
            .WithColumn(Tables.Snapshots.Version).AsInt64().NotNullable().PrimaryKey()
            .WithColumn(Tables.Snapshots.Payload).AsString(Tables.Snapshots.PayloadLength).NotNullable()
            .WithColumn(Tables.Snapshots.Timestamp).AsDateTime().NotNullable()
            .WithColumn(Tables.Snapshots.AggregateKey).AsString(Tables.Snapshots.AggregateKeyLength).NotNullable();

        Create.Table(Tables.OutboxEnvelopes.Table)
            .WithColumn(Tables.OutboxEnvelopes.StreamId).AsString(Tables.OutboxEnvelopes.StreamIdLength).NotNullable().PrimaryKey()
            .WithColumn(Tables.OutboxEnvelopes.Version).AsInt64().NotNullable().PrimaryKey()
            .WithColumn(Tables.OutboxEnvelopes.Payload).AsString(Tables.OutboxEnvelopes.PayloadLength).NotNullable()
            .WithColumn(Tables.OutboxEnvelopes.Timestamp).AsDateTime().NotNullable()
            .WithColumn(Tables.OutboxEnvelopes.EventKey).AsString(Tables.OutboxEnvelopes.EventKeyLength).NotNullable();

        // SQLite doesnt support the foreign key syntax of FluentMigrator
        Execute.Sql($"""
                     CREATE TABLE "{Tables.OutboxEnvelopeConsumers.Table}" (
                         "{Tables.OutboxEnvelopeConsumers.StreamId}" TEXT NOT NULL,
                         "{Tables.OutboxEnvelopeConsumers.Version}" INTEGER NOT NULL,
                         "{Tables.OutboxEnvelopeConsumers.ConsumerKey}" TEXT NOT NULL,
                         "{Tables.OutboxEnvelopeConsumers.Type}" INTEGER NOT NULL,
                         "{Tables.OutboxEnvelopeConsumers.ConsumedAt}" DATETIME NULL,
                         "{Tables.OutboxEnvelopeConsumers.FailedAt}" DATETIME NULL,

                         PRIMARY KEY (
                             "{Tables.OutboxEnvelopeConsumers.StreamId}",
                             "{Tables.OutboxEnvelopeConsumers.Version}",
                             "{Tables.OutboxEnvelopeConsumers.ConsumerKey}"
                         ),

                         FOREIGN KEY (
                             "{Tables.OutboxEnvelopeConsumers.StreamId}",
                             "{Tables.OutboxEnvelopeConsumers.Version}"
                         )
                         REFERENCES "{Tables.OutboxEnvelopes.Table}" (
                             "{Tables.OutboxEnvelopes.StreamId}",
                             "{Tables.OutboxEnvelopes.Version}"
                         )
                     );
                     """);
    }

    public override void Down()
    {
        Delete.Table(Tables.OutboxEnvelopeConsumers.Table);
        Delete.Table(Tables.OutboxEnvelopes.Table);
        Delete.Table(Tables.Snapshots.Table);
        Delete.Table(Tables.Envelopes.Table);
    }
}