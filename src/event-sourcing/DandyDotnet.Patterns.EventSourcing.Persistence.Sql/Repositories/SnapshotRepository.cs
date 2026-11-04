using System.Data;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using Dapper;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Repositories;

internal sealed class SnapshotRepository(
    SqlStrings sqlStrings,
    IReadOnlyUnitOfWorkContext unitOfWorkContext) : ISnapshotRepository
{
    public async Task<SnapshotEntity?> GetLatestSnapshotAsync(string streamId, long? toVersion, CancellationToken cancellationToken)
    {
        var raw = await unitOfWorkContext.Connection.QueryAsync<SnapshotEntity>(new CommandDefinition(
            sqlStrings.GetLastSnapshot,
            new { StreamId = streamId, ToVersion = toVersion },
            transaction: unitOfWorkContext.Transaction,
            cancellationToken: cancellationToken));

        return raw.FirstOrDefault();
    }

    public async Task<Dictionary<string, SnapshotEntity>> GetLatestSnapshotsAsync(string[]? streamIds, string? aggregateKey, CancellationToken cancellationToken)
    {
        if (streamIds is { Length: 0 })
            return [];

        var sql = sqlStrings.GetLatestSnapshots;
        var parameters = new DynamicParameters();
        parameters.Add("AggregateKey", aggregateKey, DbType.String);
        if (streamIds is not null)
        {
            var ids = streamIds.Distinct().ToArray();
            var names = new string[ids.Length];
            for (var i = 0; i < ids.Length; i++)
            {
                names[i] = $"@StreamId{i}";
                parameters.Add($"StreamId{i}", ids[i]);
            }
            sql += $" AND s.{Tables.Snapshots.StreamId} IN ({string.Join(", ", names)})";
        }

        var rows = await unitOfWorkContext.Connection.QueryAsync<SnapshotEntity>(new CommandDefinition(
            sql,
            parameters,
            transaction: unitOfWorkContext.Transaction,
            cancellationToken: cancellationToken));

        return rows.ToDictionary(snapshot => snapshot.StreamId, StringComparer.Ordinal);
    }

    public async Task InsertAsync(SnapshotEntity snapshotEntity, CancellationToken cancellationToken)
    {
        await unitOfWorkContext.Connection.ExecuteAsync(new CommandDefinition(
            sqlStrings.StoreSnapshot,
            snapshotEntity,
            transaction: unitOfWorkContext.Transaction,
            cancellationToken: cancellationToken));
    }
}