using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Migrations;

internal sealed class Migration_20260907191400 : IMigration
{
    public long Version => 20260907191400;

    public void Up(IMigrationBuilder builder)
    {
        builder.Execute($"""
                         IF SCHEMA_ID(N'{Sql.Constants.Schema.Name}') IS NULL
                             EXEC(N'CREATE SCHEMA [{Sql.Constants.Schema.Name}]');

                         CREATE TABLE [{Sql.Constants.Schema.Name}].[{Tables.Envelopes.Table}] (
                             [{Tables.Envelopes.StreamId}] NVARCHAR({Tables.Envelopes.StreamIdLength}) NOT NULL,
                             [{Tables.Envelopes.Payload}] NVARCHAR(MAX) NOT NULL,
                             [{Tables.Envelopes.Version}] BIGINT NOT NULL,
                             [{Tables.Envelopes.Timestamp}] DATETIME2 NOT NULL,
                             [{Tables.Envelopes.EventKey}] NVARCHAR({Tables.Envelopes.EventKeyLength}) NOT NULL,
                             CONSTRAINT [pk_envelopes] PRIMARY KEY ([{Tables.Envelopes.StreamId}], [{Tables.Envelopes.Version}])
                         );

                         CREATE TABLE [{Sql.Constants.Schema.Name}].[{Tables.Snapshots.Table}] (
                             [{Tables.Snapshots.StreamId}] NVARCHAR({Tables.Snapshots.StreamIdLength}) NOT NULL,
                             [{Tables.Snapshots.Payload}] NVARCHAR(MAX) NOT NULL,
                             [{Tables.Snapshots.Version}] BIGINT NOT NULL,
                             [{Tables.Snapshots.Timestamp}] DATETIME2 NOT NULL,
                             [{Tables.Snapshots.AggregateKey}] NVARCHAR({Tables.Snapshots.AggregateKeyLength}) NOT NULL,
                             CONSTRAINT [pk_snapshots] PRIMARY KEY ([{Tables.Snapshots.StreamId}], [{Tables.Snapshots.Version}])
                         );

                         CREATE TABLE [{Sql.Constants.Schema.Name}].[{Tables.OutboxEnvelopes.Table}] (
                             [{Tables.OutboxEnvelopes.StreamId}] NVARCHAR({Tables.OutboxEnvelopes.StreamIdLength}) NOT NULL,
                             [{Tables.OutboxEnvelopes.Version}] BIGINT NOT NULL,
                             [{Tables.OutboxEnvelopes.Payload}] NVARCHAR(MAX) NOT NULL,
                             [{Tables.OutboxEnvelopes.Timestamp}] DATETIME2 NOT NULL,
                             [{Tables.OutboxEnvelopes.EventKey}] NVARCHAR({Tables.OutboxEnvelopes.EventKeyLength}) NOT NULL,
                             CONSTRAINT [pk_outbox_envelopes] PRIMARY KEY ([{Tables.OutboxEnvelopes.StreamId}], [{Tables.OutboxEnvelopes.Version}])
                         );

                         CREATE TABLE [{Sql.Constants.Schema.Name}].[{Tables.OutboxEnvelopeConsumers.Table}] (
                             [{Tables.OutboxEnvelopeConsumers.StreamId}] NVARCHAR({Tables.OutboxEnvelopeConsumers.StreamIdLength}) NOT NULL,
                             [{Tables.OutboxEnvelopeConsumers.Version}] BIGINT NOT NULL,
                             [{Tables.OutboxEnvelopeConsumers.ConsumerKey}] NVARCHAR({Tables.OutboxEnvelopeConsumers.ConsumerKeyLength}) NOT NULL,
                             [{Tables.OutboxEnvelopeConsumers.Type}] SMALLINT NOT NULL,
                             [{Tables.OutboxEnvelopeConsumers.ConsumedAt}] DATETIME2 NULL,
                         [{Tables.OutboxEnvelopeConsumers.FailedAt}] DATETIME2 NULL,
                         [{Tables.OutboxEnvelopeConsumers.Tries}] INT NOT NULL,
                             CONSTRAINT [pk_outbox_envelope_consumers] PRIMARY KEY (
                                 [{Tables.OutboxEnvelopeConsumers.StreamId}],
                                 [{Tables.OutboxEnvelopeConsumers.Version}],
                                 [{Tables.OutboxEnvelopeConsumers.ConsumerKey}]
                             ),
                             CONSTRAINT [fk_outbox_envelope_consumers]
                                 FOREIGN KEY ([{Tables.OutboxEnvelopeConsumers.StreamId}], [{Tables.OutboxEnvelopeConsumers.Version}])
                                 REFERENCES [{Sql.Constants.Schema.Name}].[{Tables.OutboxEnvelopes.Table}] (
                                     [{Tables.OutboxEnvelopes.StreamId}], [{Tables.OutboxEnvelopes.Version}]
                                 )
                                 ON DELETE CASCADE
                         );
                         """);
    }

    public void Down(IMigrationBuilder builder)
    {
        builder.Execute($"""
                         DROP TABLE [{Sql.Constants.Schema.Name}].[{Tables.OutboxEnvelopeConsumers.Table}];
                         DROP TABLE [{Sql.Constants.Schema.Name}].[{Tables.OutboxEnvelopes.Table}];
                         DROP TABLE [{Sql.Constants.Schema.Name}].[{Tables.Snapshots.Table}];
                         DROP TABLE [{Sql.Constants.Schema.Name}].[{Tables.Envelopes.Table}];
                         DROP SCHEMA [{Sql.Constants.Schema.Name}];
                         """);
    }
}
