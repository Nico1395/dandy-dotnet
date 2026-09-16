namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;

/// <summary>
///     Provides database table and column name constants used by the SQL persistence layer.
/// </summary>
public static class Tables
{
    /// <summary>
    ///     Contains database table and column constants for event envelopes.
    /// </summary>
    public static class Envelopes
    {
        /// <summary>The database table name for event envelopes.</summary>
        public const string Table = "envelopes";

        /// <summary>The stream ID column name.</summary>
        public const string StreamId = "stream_id";
        /// <summary>The maximum length for the stream ID column.</summary>
        public const int StreamIdLength = 255;
        /// <summary>The event payload column name.</summary>
        public const string Payload = "payload";
        /// <summary>The maximum length for the payload column.</summary>
        public const int PayloadLength = int.MaxValue;
        /// <summary>The stream version column name.</summary>
        public const string Version = "version";
        /// <summary>The timestamp column name.</summary>
        public const string Timestamp = "timestamp";
        /// <summary>The event key column name.</summary>
        public const string EventKey = "event_key";
        /// <summary>The maximum length for the event key column.</summary>
        public const int EventKeyLength = 255;
    }

    /// <summary>
    ///     Contains database table and column constants for aggregate snapshots.
    /// </summary>
    public static class Snapshots
    {
        /// <summary>The database table name for aggregate snapshots.</summary>
        public const string Table = "snapshots";

        /// <summary>The stream ID column name.</summary>
        public const string StreamId = "stream_id";
        /// <summary>The maximum length for the stream ID column.</summary>
        public const int StreamIdLength = 255;
        /// <summary>The snapshot payload column name.</summary>
        public const string Payload = "payload";
        /// <summary>The maximum length for the payload column.</summary>
        public const int PayloadLength = int.MaxValue;
        /// <summary>The snapshot version column name.</summary>
        public const string Version = "version";
        /// <summary>The timestamp column name.</summary>
        public const string Timestamp = "timestamp";
        /// <summary>The aggregate key column name.</summary>
        public const string AggregateKey = "aggregate_key";
        /// <summary>The maximum length for the aggregate key column.</summary>
        public const int AggregateKeyLength = 255;
    }

    /// <summary>
    ///     Contains database table and column constants for outbox envelopes.
    /// </summary>
    public static class OutboxEnvelopes
    {
        /// <summary>The database table name for outbox envelopes.</summary>
        public const string Table = "outbox_envelopes";
        
        /// <summary>The stream ID column name.</summary>
        public const string StreamId = "stream_id";
        /// <summary>The maximum length for the stream ID column.</summary>
        public const int StreamIdLength = 255;
        /// <summary>The envelope payload column name.</summary>
        public const string Payload = "payload";
        /// <summary>The maximum length for the payload column.</summary>
        public const int PayloadLength = int.MaxValue;
        /// <summary>The stream version column name.</summary>
        public const string Version = "version";
        /// <summary>The timestamp column name.</summary>
        public const string Timestamp = "timestamp";
        /// <summary>The event key column name.</summary>
        public const string EventKey = "event_key";
        /// <summary>The maximum length for the event key column.</summary>
        public const int EventKeyLength = 255;
    }

    /// <summary>
    ///     Contains database table and column constants for outbox envelope consumer delivery tracking.
    /// </summary>
    public static class OutboxEnvelopeConsumers
    {
        /// <summary>The database table name for outbox envelope consumers.</summary>
        public const string Table = "outbox_envelope_consumers";
        
        /// <summary>The stream ID column name.</summary>
        public const string StreamId = "stream_id";
        /// <summary>The maximum length for the stream ID column.</summary>
        public const int StreamIdLength = 255;
        /// <summary>The stream version column name.</summary>
        public const string Version = "version";
        /// <summary>The consumer type column name.</summary>
        public const string Type = "type";
        /// <summary>The consumed timestamp column name.</summary>
        public const string ConsumedAt = "consumed_at";
        /// <summary>The failed timestamp column name.</summary>
        public const string FailedAt = "failed_at";
        /// <summary>The consumer key column name.</summary>
        public const string ConsumerKey = "consumer_key";
        /// <summary>The maximum length for the consumer key column.</summary>
        public const int ConsumerKeyLength = 255;
        /// <summary>The retry attempt count column name.</summary>
        public const string Tries = "tries";
    }
}