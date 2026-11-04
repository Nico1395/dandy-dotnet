using System.Diagnostics;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;
using DandyDotnet.Patterns.EventSourcing.Outbox;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;
using DandyDotnet.Patterns.EventSourcing.Persistence.Mapping;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing;

internal sealed class EventStore(
    EventStoreConfiguration eventStoreConfiguration,
    IServiceProvider serviceProvider,
    ISerializer serializer,
    IOutbox outbox,
    IUnitOfWork unitOfWork) : IEventStore
{
    public async Task<Dictionary<string, object>> ReplayAggregatesAsync(Type? aggregateType, IEnumerable<string>? streamIds, CancellationToken cancellationToken)
    {
        if (aggregateType == null && streamIds == null)
            throw new ArgumentException($"Either '{nameof(aggregateType)}' or '{nameof(streamIds)}' have to be provided.");

        // Fetching the latest snapshots
        var ids = streamIds?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct(StringComparer.Ordinal).ToArray();
        if (ids is { Length: 0 })
            return new Dictionary<string, object>();

        var aggregateConfiguration = aggregateType == null ? null : eventStoreConfiguration.Aggregates.GetOrAddAggregateConfiguration(aggregateType);
        var snapshotByStreamIds = await unitOfWork.Snapshots.GetLatestSnapshotsAsync(ids, aggregateConfiguration?.Key, cancellationToken);

        // Deserialize aggregates and keep track of versions
        var versionsByStreamId = new Dictionary<string, long?>(StringComparer.Ordinal);
        var snapshotsAndConfigsByStreamId = new Dictionary<string, (AggregateConfiguration Configuration, object? Snapshot)>(StringComparer.Ordinal);
        foreach (var snapshot in snapshotByStreamIds)
        {
            object? aggregate = null;
            if (aggregateConfiguration != null && aggregateConfiguration.UseSnapshots())
                aggregate = serializer.Deserialize(snapshot.Value.Payload, aggregateConfiguration.RuntimeType);

            versionsByStreamId[snapshot.Value.StreamId] = aggregate == null ? null : snapshot.Value.Version;
            if (aggregateConfiguration != null)
                snapshotsAndConfigsByStreamId[snapshot.Value.StreamId] = (aggregateConfiguration, aggregate);
        }

        // If none queried we really don't need to continue
        if (versionsByStreamId.Count == 0 || snapshotsAndConfigsByStreamId.Count == 0)
            return new Dictionary<string, object>();

        // Querying remaining envelopes
        var envelopeEntities = await unitOfWork.Envelopes.GetEnvelopesAfterSnapshotsAsync(versionsByStreamId, cancellationToken);
        var envelopesByStreamId = InternalMapper.MapFromEntity(eventStoreConfiguration, serializer, envelopeEntities)
            .GroupBy(envelope => envelope.StreamId, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.OrderBy(envelope => envelope.Version).ToArray(), StringComparer.Ordinal);

        // Orchestrate replay for every aggregate
        var result = new Dictionary<string, object>(StringComparer.Ordinal);
        foreach (var streamId in versionsByStreamId.Keys)
        {
            var envelopes = envelopesByStreamId.TryGetValue(streamId, out var env) ? env : [];

            if (!snapshotsAndConfigsByStreamId.TryGetValue(streamId, out var snapshotAndConfig))
            {
                if (envelopes.Length != 0)
                    throw new Abstractions.AggregateException($"Cannot determine aggregate type for stream '{streamId}' without a snapshot.");

                continue;
            }

            // Neither snapshot nor events, so there is nothing to replay here
            if (snapshotAndConfig.Snapshot == null && envelopes.Length == 0)
                continue;

            // Validate for version duplicates
            if (envelopes.GroupBy(envelope => envelope.Version).Any(group => group.Count() > 1))
                throw new EventStreamException($"Stream with ID '{streamId}' has duplicate events.");

            // Replay and add if successful
            var replayed = CreateAggregate(snapshotAndConfig.Configuration, snapshotAndConfig.Snapshot, envelopes);
            if (replayed != null)
                result.Add(streamId, replayed);
        }

        return result;
    }

    public async Task<object?> ReplayAggregateAsync(Type aggregateType, string streamId, long? toVersion, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(streamId))
            return null;

        var configuration = eventStoreConfiguration.Aggregates.GetOrAddAggregateConfiguration(aggregateType);
        var (snapshot, stream) = await this.ReplayStreamAsync(streamId, toVersion, toTimestamp, cancellationToken);

        if (snapshot == null && stream.Length == 0)
            return null;

        var hasDuplicates = stream.GroupBy(e => e.Version).Any(c => c.Count() > 1);
        if (hasDuplicates)
            throw new EventStreamException($"Stream with ID '{streamId}' has duplicate events.");

        stream = stream.OrderBy(e => e.Version).ToArray();

        return CreateAggregate(configuration, snapshot?.Aggregate, stream);
    }

    private object? CreateAggregate(AggregateConfiguration configuration, object? snapshot, IReadOnlyEnvelope[] stream)
    {
        if (configuration.FactoryFunc != null)
            return configuration.FactoryFunc(snapshot, stream);

        var factoryType = typeof(IAggregateFactory<>).MakeGenericType(configuration.RuntimeType);
        var factory = serviceProvider.GetService(factoryType);
        if (factory == null)
            throw new Abstractions.AggregateException($"Could not resolve aggregate factory for aggregate of type '{configuration.RuntimeType}'.");

        var create = factoryType.GetMethod(nameof(IAggregateFactory<>.Create)) ?? throw new UnreachableException($"The aggregate factory should have a method 'Create'.");
        return create.Invoke(factory, [snapshot, stream]);
    }

    public async Task<IReadOnlyEnvelope[]> GetStreamAsync(string streamId, long? fromVersion, long? toVersion, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken)
    {
        var envelopeEntities = await unitOfWork.Envelopes.GetEnvelopesAsync(streamId, fromVersion, toVersion, fromTimestamp, toTimestamp, cancellationToken);
        if (envelopeEntities.Length == 0)
            return [];

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

    public async Task<IReadOnlyEnvelope[]> GetEnvelopesAsync(IEnumerable<string> tags, CancellationToken cancellationToken)
    {
        var processedTags = tags
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.Ordinal)
            .Order()
            .ToArray();
        if (processedTags.Length == 0)
            return [];

        var envelopeEntities = await unitOfWork.Envelopes.GetEnvelopesAsync(processedTags, cancellationToken);
        if (envelopeEntities.Length == 0)
            return [];

        var envelopes = InternalMapper.MapFromEntity(
            eventStoreConfiguration,
            serializer,
            envelopeEntities);

        return envelopes.ToArray<IReadOnlyEnvelope>();
    }

    public async Task AppendAsync(Type? aggregateType, string streamId, (object Event, IEnumerable<string>? Tags)[] events, CancellationToken cancellationToken)
    {
        if (events.Length == 0)
            return;

        if (string.IsNullOrWhiteSpace(streamId))
            throw new EventStreamException("Stream ID cannot be null or whitespace.");

        // Fetch current version
        var currentVersion = await unitOfWork.Envelopes.GetStreamVersionAsync(streamId, cancellationToken);

        // Create envelopes
        var envelopeVersion = currentVersion;
        var envelopes = events.Select(e => CreateEnvelope(streamId, e.Event, e.Tags, envelopeVersion++)).ToArray();
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
    }

    private Envelope CreateEnvelope(string streamId, object @event, IEnumerable<string>? tags, long version)
    {
        var configuration = eventStoreConfiguration.Aggregates.GetOrAddAggregateConfiguration(@event.GetType());

        string[] processedTags;
        if (tags == null)
        {
            processedTags = [];
        }
        else

        {
            processedTags = tags
                .Where(s => !string.IsNullOrWhiteSpace(s))  // Filter out null or empty tags
                .Distinct(StringComparer.Ordinal)   // Distinct case-sensitively
                .Order()    // Order alphabetically
                .ToArray();

            // Make sure to punish trying to sneak in the delimiter
            EventStreamException.ThrowIfTagsContainDelimiter(processedTags);
        }

        return new Envelope
        {
            StreamId = streamId,
            Event = @event,
            Timestamp = DateTime.UtcNow,
            Version = version,
            EventKey = configuration.Key,
            RuntimeType = configuration.RuntimeType,
            Tags = processedTags,
        };
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
                throw new Abstractions.AggregateException($"Failed to replay aggregate {aggregateType.FullName} from stream {streamId} to create snapshot.");

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
