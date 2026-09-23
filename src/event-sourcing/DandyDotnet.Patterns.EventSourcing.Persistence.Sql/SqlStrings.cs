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
                selectWhere += $"{TagsLike} @{parameterName}";
            else
                selectWhere += $"{OrTagsLike} @{parameterName}";

            index++;
        }

        return (selectWhere, parameters);
    }
}