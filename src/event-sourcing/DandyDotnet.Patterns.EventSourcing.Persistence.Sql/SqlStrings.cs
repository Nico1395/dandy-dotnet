namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
///     Represents a provider of SQL command strings for event store persistence operations.
/// </summary>
/// <remarks>
///     <para>
///         Dialect-specific implementations override these properties to provide optimized SQL statements
///         tailored to the underlying database engine (e.g., PostgreSQL, SQL Server, SQLite).
///     </para>
/// </remarks>
public abstract class SqlStrings
{
    /// <summary>
    ///     Gets the SQL query string used to retrieve the latest version number of an event stream.
    /// </summary>
    public abstract string GetStreamVersion { get; }

    /// <summary>
    ///     Gets the SQL query string used to retrieve events from a stream with optional filters.
    /// </summary>
    public abstract string GetStream  { get; }

    /// <summary>
    ///     Gets the SQL statement used to insert new event envelopes into the event store.
    /// </summary>
    public abstract string InsertEnvelope  { get; }

    /// <summary>
    ///     Gets the SQL query string used to retrieve the latest aggregate snapshot.
    /// </summary>
    public abstract string GetLastSnapshot { get; }

    /// <summary>
    ///     Gets the SQL statement used to store an aggregate snapshot.
    /// </summary>
    public abstract string StoreSnapshot { get; }

    /// <summary>
    ///     Gets the SQL query string used to fetch pending outbox envelopes.
    /// </summary>
    public abstract string GetOutboxEnvelopes { get; }

    /// <summary>
    ///     Gets the SQL statement used to insert outbox envelopes.
    /// </summary>
    public abstract string InsertOutboxEnvelopes { get; }

    /// <summary>
    ///     Gets the SQL statement used to delete processed or expired outbox envelopes.
    /// </summary>
    public abstract string DeleteOutboxEnvelopes { get; }

    /// <summary>
    ///     Gets the SQL statement used to insert outbox envelope consumer delivery records.
    /// </summary>
    public abstract string InsertOutboxEnvelopeConsumers { get; }

    /// <summary>
    ///     Gets the SQL statement used to update outbox envelope consumer delivery records.
    /// </summary>
    public abstract string UpdateOutboxEnvelopeConsumers { get; }
}