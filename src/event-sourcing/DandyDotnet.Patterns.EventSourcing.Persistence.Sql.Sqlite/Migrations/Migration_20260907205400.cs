using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Migrations;

public sealed class Migration_20260907205400 : IMigration
{
    public long Version => 20260907205400;

    public void Up(IMigrationBuilder builder)
    {
        builder.Execute($"""
                         CREATE TABLE "{Tables.Envelopes.Table}" (
                             "{Tables.Envelopes.StreamId}" TEXT NOT NULL,
                             "{Tables.Envelopes.Version}" INTEGER NOT NULL,
                             "{Tables.Envelopes.Payload}" TEXT NOT NULL,
                             "{Tables.Envelopes.Timestamp}" DATETIME NOT NULL,
                             "{Tables.Envelopes.EventKey}" TEXT NOT NULL,
                             PRIMARY KEY ("{Tables.Envelopes.StreamId}", "{Tables.Envelopes.Version}")
                         );

                         CREATE TABLE "{Tables.Snapshots.Table}" (
                             "{Tables.Snapshots.StreamId}" TEXT NOT NULL,
                             "{Tables.Snapshots.Version}" INTEGER NOT NULL,
                             "{Tables.Snapshots.Payload}" TEXT NOT NULL,
                             "{Tables.Snapshots.Timestamp}" DATETIME NOT NULL,
                             "{Tables.Snapshots.AggregateKey}" TEXT NOT NULL,
                             PRIMARY KEY ("{Tables.Snapshots.StreamId}", "{Tables.Snapshots.Version}")
                         );

                         CREATE TABLE "{Tables.OutboxEnvelopes.Table}" (
                             "{Tables.OutboxEnvelopes.StreamId}" TEXT NOT NULL,
                             "{Tables.OutboxEnvelopes.Version}" INTEGER NOT NULL,
                             "{Tables.OutboxEnvelopes.Payload}" TEXT NOT NULL,
                             "{Tables.OutboxEnvelopes.Timestamp}" DATETIME NOT NULL,
                             "{Tables.OutboxEnvelopes.EventKey}" TEXT NOT NULL,
                             PRIMARY KEY ("{Tables.OutboxEnvelopes.StreamId}", "{Tables.OutboxEnvelopes.Version}")
                         );

                         CREATE TABLE "{Tables.OutboxEnvelopeConsumers.Table}" (
                             "{Tables.OutboxEnvelopeConsumers.StreamId}" TEXT NOT NULL,
                             "{Tables.OutboxEnvelopeConsumers.Version}" INTEGER NOT NULL,
                             "{Tables.OutboxEnvelopeConsumers.ConsumerKey}" TEXT NOT NULL,
                             "{Tables.OutboxEnvelopeConsumers.Type}" INTEGER NOT NULL,
                             "{Tables.OutboxEnvelopeConsumers.ConsumedAt}" DATETIME NULL,
                             "{Tables.OutboxEnvelopeConsumers.FailedAt}" DATETIME NULL,
                             "{Tables.OutboxEnvelopeConsumers.Tries}" INTEGER NOT NULL,
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

    public void Down(IMigrationBuilder builder)
    {
        builder.Execute($"""
                         DROP TABLE "{Tables.OutboxEnvelopeConsumers.Table}";
                         DROP TABLE "{Tables.OutboxEnvelopes.Table}";
                         DROP TABLE "{Tables.Snapshots.Table}";
                         DROP TABLE "{Tables.Envelopes.Table}";
                         """);
    }
}
