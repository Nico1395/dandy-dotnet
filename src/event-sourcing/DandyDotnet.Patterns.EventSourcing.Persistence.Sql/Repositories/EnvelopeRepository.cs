using System.Text;
using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;
using Dapper;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Repositories;

internal sealed class EnvelopeRepository(
    SqlStrings sqlStrings,
    IReadOnlyUnitOfWorkContext unitOfWorkContext) : IEnvelopeRepository
{
    public async Task<long> GetStreamVersionAsync(string streamId, CancellationToken cancellationToken)
    {
        return await unitOfWorkContext.Connection.ExecuteScalarAsync<long>(new CommandDefinition(
            sqlStrings.GetStreamVersion,
            new { StreamId = streamId },
            transaction: unitOfWorkContext.Transaction,
            cancellationToken: cancellationToken));
    }

    public async Task<EnvelopeEntity[]> GetStreamAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        var rows = await unitOfWorkContext.Connection.QueryAsync<EnvelopeRow>(new CommandDefinition(
            sqlStrings.GetStream,
            new
            {
                StreamId = streamId,
                FromVersion = fromVersion,
                ToVersion = toVersion,
                FromTimestamp = fromTimestamp,
                ToTimestamp = toTimestamp,
            },
            transaction: unitOfWorkContext.Transaction,
            cancellationToken: cancellationToken));

        return GetEntities(rows).ToArray();
    }

    public async Task InsertAsync(string streamId, EnvelopeEntity[] envelopes, CancellationToken cancellationToken)
    {
        if (envelopes.Length == 0)
            return;

        var parameters = envelopes.Select(e => new
        {
            e.StreamId,
            e.Version,
            e.Timestamp,
            e.EventKey,
            e.Payload,
            Tags = TagsToString(e.Tags),
        });

        await unitOfWorkContext.Connection.ExecuteAsync(new CommandDefinition(
            sqlStrings.InsertEnvelope,
            parameters,
            transaction: unitOfWorkContext.Transaction,
            cancellationToken: cancellationToken));
    }

    private static IEnumerable<EnvelopeEntity> GetEntities(IEnumerable<EnvelopeRow> rows)
    {
        return rows.Select(row => new EnvelopeEntity
        {
            StreamId = row.StreamId,
            Payload = row.Payload,
            Version = row.Version,
            Timestamp = row.Timestamp,
            EventKey = row.EventKey,
            Tags = StringToTags(row.Tags),
        });
    }

    private static string? TagsToString(string[] tags)
    {
        if (tags.Length == 0)
            return null;

        var builder = new StringBuilder()
            .Append(';')
            .Append(string.Join(';', tags))
            .Append(';');

        return builder.ToString();
    }

    private static string[] StringToTags(string? tagsString)
    {
        if (string.IsNullOrWhiteSpace(tagsString))
            return [];

        return tagsString.Trim(';').Split(';');
    }

    private sealed class EnvelopeRow
    {
        public required string StreamId { get; init; }
        public required string Payload { get; init; }
        public required long Version { get; init; }
        public required DateTime Timestamp { get; init; }
        public required string EventKey { get; init; }
        public required string? Tags { get; init; }
    }
}