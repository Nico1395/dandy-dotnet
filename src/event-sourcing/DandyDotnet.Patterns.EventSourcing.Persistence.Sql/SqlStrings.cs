using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using Dapper;

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
    ///     Gets the SQL query string used to retrieve envelopes from a stream with optional filters.
    /// </summary>
    public abstract string GetStream  { get; }

    /// <summary>
    ///     Gets the SQL query string used to select envelopes.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Ends with <c>WHERE</c>.
    ///     </para>
    ///     <para>
    ///         This is used in combination with <see cref="TagsLike"/> and <see cref="OrTagsLike"/> to
    ///         query for a dynamic number of tags.
    ///     </para>
    /// </remarks>
    public abstract string SelectEnvelopesWhere  { get; }

    /// <summary>
    ///     Gets the SQL string for a condition within a where statement that checks for a given tag.
    /// </summary>
    public abstract string TagsLike { get; }

    /// <summary>
    ///     Gets the SQL string for a condition within a where statement that checks for a given tag.
    /// </summary>
    public abstract string OrTagsLike { get; }

    /// <summary>
    ///     Gets the SQL statement used to insert new event envelopes into the event store.
    /// </summary>
    public abstract string InsertEnvelope  { get; }

    /// <summary>
    ///     Gets the SQL query string used to retrieve the latest aggregate snapshot.
    /// </summary>
    public abstract string GetLastSnapshot { get; }

    /// <summary>
    ///     The qualified snapshots table name.
    /// </summary>
    protected virtual string SnapshotsTable => Tables.Snapshots.Table;

    /// <summary>
    ///     Gets the latest matching snapshot for each requested stream.
    /// </summary>
    public virtual string GetLatestSnapshots => $"""
        SELECT s.{Tables.Snapshots.StreamId} AS StreamId,
               s.{Tables.Snapshots.Payload} AS Payload,
               s.{Tables.Snapshots.Version} AS Version,
               s.{Tables.Snapshots.Timestamp} AS Timestamp,
               s.{Tables.Snapshots.AggregateKey} AS AggregateKey
        FROM {SnapshotsTable} s
        WHERE (@AggregateKey IS NULL OR s.{Tables.Snapshots.AggregateKey} = @AggregateKey)
          AND s.{Tables.Snapshots.Version} = (
              SELECT MAX(latest.{Tables.Snapshots.Version}) FROM {SnapshotsTable} latest
              WHERE latest.{Tables.Snapshots.StreamId} = s.{Tables.Snapshots.StreamId}
                AND (@AggregateKey IS NULL OR latest.{Tables.Snapshots.AggregateKey} = @AggregateKey))
        """;

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

    /// <summary>
    /// Builds SQL query string and parameters for querying envelopes by the given <paramref name="tags"/>.
    /// </summary>
    /// <param name="tags">Tags to query for.</param>
    /// <returns></returns>
    public virtual (string? Sql, DynamicParameters Parameters) GetEnvelopesByTags(string[] tags)
    {
        if (tags.Length == 0)
            return (null, new DynamicParameters());

        var selectWhere = SelectEnvelopesWhere;
        var parameters = new DynamicParameters();

        var index = 0;
        foreach (var tag in tags)
        {
            var parameterName = $"Tag{index}";
            parameters.Add(parameterName, $"%;{tag};%");

            if (index == 0)
                selectWhere += $" {TagsLike}@{parameterName}";
            else
                selectWhere += $" {OrTagsLike}@{parameterName}";

            index++;
        }

        return (selectWhere, parameters);
    }

    /// <summary>Builds a single query for envelopes after each stream's snapshot version.</summary>
    /// <param name="snapshotVersions">Snapshot versions by stream; null means no snapshot.</param>
    /// <returns>The query and parameters, or a null query when no streams are requested.</returns>
    public virtual (string? Sql, DynamicParameters Parameters) GetEnvelopesAfterSnapshots(IReadOnlyDictionary<string, long?> snapshotVersions)
    {
        var parameters = new DynamicParameters();
        if (snapshotVersions.Count == 0)
            return (null, parameters);

        var conditions = new List<string>(snapshotVersions.Count);
        var index = 0;
        foreach (var (streamId, version) in snapshotVersions)
        {
            parameters.Add($"StreamId{index}", streamId);
            var condition = $"{Tables.Envelopes.StreamId} = @StreamId{index}";
            if (version.HasValue)
            {
                parameters.Add($"Version{index}", version.Value);
                condition += $" AND {Tables.Envelopes.Version} > @Version{index}";
            }
            conditions.Add($"({condition})");
            index++;
        }

        return ($"{SelectEnvelopesWhere} {string.Join(" OR ", conditions)} ORDER BY {Tables.Envelopes.StreamId}, {Tables.Envelopes.Version}", parameters);
    }
}