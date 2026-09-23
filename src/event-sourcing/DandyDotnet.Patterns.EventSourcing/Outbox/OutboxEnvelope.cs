using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Outbox;

/// <inheritdoc/>
public sealed class OutboxEnvelope : IReadOnlyOutboxEnvelope
{
    /// <inheritdoc/>
    public required string StreamId { get; init; }
    
    /// <inheritdoc/>
    public required object Event { get; init; }
    
    /// <inheritdoc/>
    public required long Version { get; init; }
    
    /// <inheritdoc/>
    public required DateTime Timestamp { get; init; }
    
    /// <inheritdoc/>
    public required string EventKey { get; init; }
    
    /// <inheritdoc/>
    public required Type RuntimeType { get; init; }
    
    /// <summary>
    /// Consumers of the envelope.
    /// </summary>
    public List<OutboxEnvelopeConsumer> Consumers { get; init; } = [];

    /// <summary>
    /// Creates an outbox envelope for an <paramref name="envelope"/>.
    /// </summary>
    /// <param name="envelope">The envelope to create an outbox envelope for.</param>
    /// <returns>The outbox envelope.</returns>
    public static OutboxEnvelope Create(IReadOnlyEnvelope envelope)
    {
        return new OutboxEnvelope
        {
            StreamId = envelope.StreamId,
            Event = envelope.Event,
            Version = envelope.Version,
            Timestamp = envelope.Timestamp,
            EventKey = envelope.EventKey,
            RuntimeType = envelope.RuntimeType,
        };
    }

    /// <summary>
    /// Determines whether the envelope is expired.
    /// </summary>
    /// <param name="eventLifetime">Lifetime of the event.</param>
    /// <returns><see langword="true"/> if the envelope is expired.</returns>
    public bool IsExpired(TimeSpan eventLifetime)
    {
        return DateTime.UtcNow >= Timestamp + eventLifetime;
    }

    /// <summary>
    /// Marks the consumer with the <paramref name="consumerKey"/> to have successfully consumed the envelopes <see cref="Event"/>.
    /// </summary>
    /// <param name="consumerKey">Unique key of the consumer.</param>
    /// <param name="type">Type of the consumer.</param>
    public void Consume(string consumerKey, OutboxEventConsumerType type)
    {
        var consumer = Consumers.SingleOrDefault(c => c.ConsumerKey == consumerKey && !c.ConsumedAt.HasValue);
        if (consumer == null)
        {
            Consumers.Add(consumer = new OutboxEnvelopeConsumer
            {
                StreamId = StreamId,
                Version = Version,
                ConsumerKey = consumerKey,
                Type = type,
                IsNew = true,
            });
        }

        consumer.Consume();
    }

    /// <summary>
    /// Marks the consumer with the <paramref name="consumerKey"/> to have failed consuming the envelopes <see cref="Event"/>.
    /// </summary>
    /// <param name="consumerKey">Unique key of the consumer.</param>
    /// <param name="type">Type of the consumer.</param>
    public void Fail(string consumerKey, OutboxEventConsumerType type)
    {
        var consumer = Consumers.SingleOrDefault(c => c.ConsumerKey == consumerKey && !c.ConsumedAt.HasValue);
        if (consumer == null)
        {
            Consumers.Add(consumer = new OutboxEnvelopeConsumer
            {
                StreamId = StreamId,
                Version = Version,
                ConsumerKey = consumerKey,
                Type = type,
                IsNew = true,
            });
        }

        consumer.Fail();
    }

    /// <summary>
    /// Determines if the consumer with the <paramref name="consumerKey"/> has successfully consumed the envelopes <see cref="Event"/>.
    /// </summary>
    /// <param name="consumerKey">Unique key of the consumer.</param>
    /// <returns><see langword="true"/> if the consumer has successfully consumed the envelopes <see cref="Event"/> already.</returns>
    public bool HasConsumed(string consumerKey)
    {
        return Consumers.Any(c => c.ConsumerKey == consumerKey && c.ConsumedAt.HasValue);
    }
}