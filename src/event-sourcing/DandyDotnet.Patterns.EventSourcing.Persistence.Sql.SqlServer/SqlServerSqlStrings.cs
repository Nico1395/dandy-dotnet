namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

internal sealed class SqlServerSqlStrings : SqlStrings
{
    public override string GetStreamVersion { get; }
    public override string GetStream { get; }
    public override string InsertEnvelope { get; }
    public override string GetLastSnapshot { get; }
    public override string StoreSnapshot { get; }
    public override string GetOutboxEnvelopes { get; }
    public override string InsertOutboxEnvelopes { get; }
    public override string DeleteOutboxEnvelopes { get; }
    public override string InsertOutboxEnvelopeConsumers { get; }
    public override string UpdateOutboxEnvelopeConsumers { get; }
}