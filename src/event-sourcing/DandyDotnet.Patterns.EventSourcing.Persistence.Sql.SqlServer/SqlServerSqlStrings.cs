using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

internal sealed class SqlServerSqlStrings : SqlStrings
{
    public override string GetStreamVersion => $"""
                                                    SELECT COALESCE(MAX(Version) + 1, 0)
                                                    FROM {Schema.Name}.{Tables.Envelopes.Table}
                                                    WHERE {Tables.Envelopes.StreamId} = @StreamId
                                                """;

    public override string GetStream => $"""
                                              SELECT
                                                  {Tables.Envelopes.StreamId} AS StreamId,
                                                  {Tables.Envelopes.Payload} AS Payload,
                                                  {Tables.Envelopes.Version} AS Version,
                                                  {Tables.Envelopes.Timestamp} AS Timestamp,
                                                  {Tables.Envelopes.EventKey} AS EventKey
                                              FROM {Schema.Name}.{Tables.Envelopes.Table}
                                              WHERE {Tables.Envelopes.StreamId} = @StreamId
                                              AND (CAST(@FromVersion AS BIGINT) IS NULL OR {Tables.Envelopes.Version} >= CAST(@FromVersion AS BIGINT))
                                              AND (CAST(@ToVersion AS BIGINT) IS NULL OR {Tables.Envelopes.Version} <= CAST(@ToVersion AS BIGINT))
                                              AND (CAST(@FromTimestamp AS DATETIME2) IS NULL OR {Tables.Envelopes.Timestamp} >= CAST(@FromTimestamp AS DATETIME2))
                                              AND (CAST(@ToTimestamp AS DATETIME2) IS NULL OR {Tables.Envelopes.Timestamp} <= CAST(@ToTimestamp AS DATETIME2))
                                          """;

    public override string InsertEnvelope => $"""
                                                 INSERT INTO {Schema.Name}.{Tables.Envelopes.Table} (
                                                     {Tables.Envelopes.StreamId},
                                                     {Tables.Envelopes.Payload},
                                                     {Tables.Envelopes.Version},
                                                     {Tables.Envelopes.Timestamp},
                                                     {Tables.Envelopes.EventKey})
                                                 VALUES (
                                                     @StreamId,
                                                     @Payload,
                                                     @Version,
                                                     @Timestamp,
                                                     @EventKey)
                                             """;

    public override string GetLastSnapshot => $"""
                                                   SELECT TOP (1)
                                                       {Tables.Snapshots.StreamId} AS StreamId,
                                                       {Tables.Snapshots.Payload} AS Payload,
                                                       {Tables.Snapshots.Version} AS Version,
                                                       {Tables.Snapshots.Timestamp} AS Timestamp,
                                                       {Tables.Snapshots.AggregateKey} AS AggregateKey
                                                   FROM {Schema.Name}.{Tables.Snapshots.Table}
                                                   WHERE {Tables.Snapshots.StreamId} = @StreamId AND (CAST(@ToVersion AS BIGINT) IS NULL OR {Tables.Snapshots.Version} <= CAST(@ToVersion AS BIGINT))
                                                   ORDER BY {Tables.Snapshots.Version} DESC
                                               """;

    public override string StoreSnapshot => $"""
                                                 INSERT INTO {Schema.Name}.{Tables.Snapshots.Table} (
                                                     {Tables.Snapshots.StreamId},
                                                     {Tables.Snapshots.Payload},
                                                     {Tables.Snapshots.Version},
                                                     {Tables.Snapshots.Timestamp},
                                                     {Tables.Snapshots.AggregateKey})
                                                 VALUES (
                                                     @StreamId,
                                                     @Payload,
                                                     @Version,
                                                     @Timestamp,
                                                     @AggregateKey)
                                             """;

    public override string GetOutboxEnvelopes => $"""
                                                        SELECT
                                                            e.{Tables.OutboxEnvelopes.StreamId} AS StreamId,
                                                            e.{Tables.OutboxEnvelopes.Payload} AS Payload,
                                                            e.{Tables.OutboxEnvelopes.Version} AS Version,
                                                            e.{Tables.OutboxEnvelopes.Timestamp} AS Timestamp,
                                                            e.{Tables.OutboxEnvelopes.EventKey} AS EventKey,
                                                            c.{Tables.OutboxEnvelopeConsumers.StreamId} AS ConsumerStreamId,
                                                            c.{Tables.OutboxEnvelopeConsumers.Version} AS ConsumerVersion,
                                                            c.{Tables.OutboxEnvelopeConsumers.ConsumerKey} AS ConsumerKey,
                                                            c.{Tables.OutboxEnvelopeConsumers.Type} AS ConsumerType,
                                                            c.{Tables.OutboxEnvelopeConsumers.ConsumedAt} AS ConsumedAt,
                                                            c.{Tables.OutboxEnvelopeConsumers.FailedAt} AS FailedAt,
                                                            c.{Tables.OutboxEnvelopeConsumers.Tries} AS Tries
                                                        FROM {Schema.Name}.{Tables.OutboxEnvelopes.Table} e
                                                        LEFT JOIN {Schema.Name}.{Tables.OutboxEnvelopeConsumers.Table} c
                                                            ON c.{Tables.OutboxEnvelopeConsumers.StreamId} = e.{Tables.OutboxEnvelopes.StreamId}
                                                            AND c.{Tables.OutboxEnvelopeConsumers.Version} = e.{Tables.OutboxEnvelopes.Version}
                                                    """;

    public override string InsertOutboxEnvelopes => $"""
                                                         INSERT INTO {Schema.Name}.{Tables.OutboxEnvelopes.Table} (
                                                             {Tables.OutboxEnvelopes.StreamId},
                                                             {Tables.OutboxEnvelopes.Payload},
                                                             {Tables.OutboxEnvelopes.Version},
                                                             {Tables.OutboxEnvelopes.Timestamp},
                                                             {Tables.OutboxEnvelopes.EventKey})
                                                         VALUES (
                                                             @StreamId,
                                                             @Payload,
                                                             @Version,
                                                             @Timestamp,
                                                             @EventKey)
                                                     """;

    public override string DeleteOutboxEnvelopes => $"""
                                                         DELETE FROM {Schema.Name}.{Tables.OutboxEnvelopeConsumers.Table}
                                                         WHERE {Tables.OutboxEnvelopeConsumers.StreamId} = @StreamId
                                                         AND {Tables.OutboxEnvelopeConsumers.Version} = @Version;
                                                         DELETE FROM {Schema.Name}.{Tables.OutboxEnvelopes.Table}
                                                         WHERE {Tables.OutboxEnvelopes.StreamId} = @StreamId
                                                         AND {Tables.OutboxEnvelopes.Version} = @Version
                                                     """;

    public override string InsertOutboxEnvelopeConsumers => $"""
                                                                INSERT INTO {Schema.Name}.{Tables.OutboxEnvelopeConsumers.Table} (
                                                                    {Tables.OutboxEnvelopeConsumers.StreamId},
                                                                    {Tables.OutboxEnvelopeConsumers.Version},
                                                                    {Tables.OutboxEnvelopeConsumers.ConsumerKey},
                                                                    {Tables.OutboxEnvelopeConsumers.Type},
                                                                    {Tables.OutboxEnvelopeConsumers.ConsumedAt},
                                                                    {Tables.OutboxEnvelopeConsumers.FailedAt},
                                                                    {Tables.OutboxEnvelopeConsumers.Tries})
                                                                VALUES (
                                                                    @StreamId,
                                                                    @Version,
                                                                    @ConsumerKey,
                                                                    @Type,
                                                                    @ConsumedAt,
                                                                    @FailedAt,
                                                                    @Tries)
                                                            """;

    public override string UpdateOutboxEnvelopeConsumers => $"""
                                                                UPDATE {Schema.Name}.{Tables.OutboxEnvelopeConsumers.Table}
                                                                SET
                                                                    {Tables.OutboxEnvelopeConsumers.ConsumedAt} = @ConsumedAt,
                                                                    {Tables.OutboxEnvelopeConsumers.FailedAt} = @FailedAt,
                                                                    {Tables.OutboxEnvelopeConsumers.Tries} = @Tries
                                                                WHERE {Tables.OutboxEnvelopeConsumers.StreamId} = @StreamId
                                                                AND {Tables.OutboxEnvelopeConsumers.Version} = @Version
                                                                AND {Tables.OutboxEnvelopeConsumers.ConsumerKey} = @ConsumerKey
                                                            """;
}
