using System.Diagnostics;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Outbox;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;
using DandyDotnet.Patterns.EventSourcing.Persistence.Mapping;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing;

internal sealed class EventStore(
    EventStoreConfiguration eventStoreConfiguration,
    IServiceProvider serviceProvider,
    IEnvelopeFactory envelopeFactory,
    ISerializer serializer,
    IOutbox outbox,
    IUnitOfWork unitOfWork) : IEventStore
{
    public async Task<object?> ReplayAggregateAsync(Type aggregateType, string streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        var configuration = eventStoreConfiguration.Aggregates.GetOrAddAggregateConfiguration(aggregateType);
        var (snapshot, stream) = await this.ReplayStreamAsync(streamId, toVersion, toTimestamp, cancellationToken);

        if (snapshot == null && stream.Length == 0)
            return null;

        var hasDuplicates = stream.GroupBy(e => e.Version).Any(c => c.Count() > 1);
        if (hasDuplicates)
            throw new InvalidOperationException($"Stream with ID '{streamId}' has duplicate events.");

        stream = stream.OrderBy(e => e.Version).ToArray();

        if (configuration.FactoryFunc != null)
            return configuration.FactoryFunc(snapshot?.Aggregate, stream);

        var factoryType = typeof(IAggregateFactory<>).MakeGenericType(configuration.RuntimeType);
        var factory = serviceProvider.GetService(factoryType);
        if (factory == null)
            throw new InvalidOperationException($"Could not resolve aggregate factory for aggregate of type '{configuration.RuntimeType}'.");

        var create = factoryType.GetMethod(nameof(IAggregateFactory<>.Create)) ?? throw new UnreachableException();
        return create.Invoke(factory, [snapshot?.Aggregate, stream]);
    }

    public async Task<IReadOnlyEnvelope[]> GetStreamAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        var envelopeEntities = await unitOfWork.Envelopes.GetStreamAsync(streamId, fromVersion, toVersion, fromTimestamp, toTimestamp, cancellationToken);
        var envelopes = InternalMapper.MapFromEntity(
            eventStoreConfiguration,
            serializer,
            envelopeEntities);

        return envelopes.ToArray<IReadOnlyEnvelope>();
    }

    public async Task<IReadOnlySnapshot?> GetLastSnapshotAsync(string streamId, long? version, CancellationToken cancellationToken)
    {
        var snapshotEntity = await unitOfWork.Snapshots.GetLatestSnapshotAsync(streamId, version, cancellationToken);
        if (snapshotEntity == null)
            return null;

        if (!eventStoreConfiguration.Aggregates.AggregatesByKey.TryGetValue(snapshotEntity.AggregateKey, out var configuration) || !configuration.UseSnapshots())
            return null;

        var aggregate = serializer.Deserialize(snapshotEntity.Payload, configuration.RuntimeType);
        if (aggregate == null)
            return null;

        return new Snapshot
        {
            StreamId = snapshotEntity.StreamId,
            Version = snapshotEntity.Version,
            Timestamp = snapshotEntity.Timestamp,
            Aggregate = aggregate,
            AggregateKey = snapshotEntity.AggregateKey,
            RuntimeType = configuration.RuntimeType,
        };
    }

    public async Task AppendAsync(Type? aggregateType, string streamId, object[] events, CancellationToken cancellationToken)
    {
        if (events.Length == 0)
            return;

        if (string.IsNullOrWhiteSpace(streamId))
            throw new ArgumentException("Stream ID cannot be null or whitespace.", nameof(streamId));

        // Fetch current version
        var currentVersion = await unitOfWork.Envelopes.GetStreamVersionAsync(streamId, cancellationToken);

        // Create envelopes
        var envelopeVersion = currentVersion;
        var envelopes = events.Select(e => envelopeFactory.Create(streamId, e, envelopeVersion++)).ToArray();
        var envelopeEntities = InternalMapper.MapToEntity(serializer, envelopes).ToArray();

        // Insert events
        await unitOfWork.Envelopes.InsertAsync(streamId, envelopeEntities, cancellationToken);

        // Publish outbox envelopes
        var outboxEnvelopes = envelopes.Select(OutboxEnvelope.Create).ToArray();
        await outbox.PublishAsync(outboxEnvelopes, cancellationToken);

        // Create and insert a snapshot if configured
        await CreateSnapshotAsync(aggregateType, streamId, envelopes, currentVersion, cancellationToken);

        // Commit transaction so event-store and outbox are in sync. Consumers should not be in this transaction.
        await unitOfWork.CommitAsync(cancellationToken);

        // Notify inline subscribers
        await outbox.NotifyInlineConsumersAsync(outboxEnvelopes, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task CreateSnapshotAsync(Type? aggregateType, string streamId, Envelope[] envelopes, long currentVersion, CancellationToken cancellationToken)
    {
        if (aggregateType == null || envelopes.Length == 0)
            return;

        var aggregateConfiguration = eventStoreConfiguration.Aggregates.GetOrAddAggregateConfiguration(aggregateType);
        var versionAfterAppend = envelopes.OrderByDescending(e => e.Version).First().Version;

        if (aggregateConfiguration.ShouldCreateSnapshot(currentVersion, versionAfterAppend))
        {
            var aggregate = await ReplayAggregateAsync(aggregateType, streamId, null, null, cancellationToken);
            if (aggregate == null)
                throw new InvalidOperationException($"Failed to replay aggregate {aggregateType.FullName} from stream {streamId} to create snapshot.");

            var snapshotEntity = new SnapshotEntity
            {
                StreamId = streamId,
                Version = versionAfterAppend,
                Timestamp = DateTime.UtcNow,
                Payload = serializer.Serialize(aggregate, aggregateType),
                AggregateKey = aggregateConfiguration.Key,
            };

            await unitOfWork.Snapshots.InsertAsync(snapshotEntity, cancellationToken);
        }
    }
}
