namespace DandyDotnet.Patterns.EventSourcing.Outbox;

/// <summary>
/// Consumer entry for an <see cref="OutboxEnvelope"/>.
/// </summary>
public sealed class OutboxEnvelopeConsumer
{
    /// <summary>
    /// ID of the stream the <see cref="OutboxEnvelope.Event"/> belongs to.
    /// </summary>
    public required string StreamId { get; init; }
    
    /// <summary>
    /// Version of the <see cref="OutboxEnvelope.Event"/>.
    /// </summary>
    public required long Version { get; init; }
    
    /// <summary>
    /// Unique consumer key.
    /// </summary>
    public required string ConsumerKey { get; init; }
    
    /// <summary>
    /// Type of the consumer.
    /// </summary>
    public required OutboxEventConsumerType Type { get; init; }

    /// <summary>
    /// Timestamp of when the <see cref="OutboxEnvelope.Event"/> was consumed by the consumer.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see langword="null"/> if not successfully consumed yet.
    ///     </para>
    /// </remarks>
    public DateTime? ConsumedAt { get; set; }

    /// <summary>
    /// Timestamp of when consumption of the <see cref="OutboxEnvelope.Event"/> was failed by the consumer.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see langword="null"/> if the consumption was not failed.
    ///     </para>
    /// </remarks>
    public DateTime? FailedAt { get; set; }

    /// <summary>
    /// Consumption attempts of the consumer.
    /// </summary>
    public int Tries { get; set; }

    internal bool IsNew { get; init; }

    /// <summary>
    /// Marks the consumer as successfully consumed.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Increases the consumer's <see cref="Tries"/> by one.
    ///     </para>
    /// </remarks>
    public void Consume()
    {
        Try();
        ConsumedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the consumer as failed.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Increases the consumer's <see cref="Tries"/> by one.
    ///     </para>
    /// </remarks>
    public void Fail()
    {
        Try();
        FailedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Determines whether the consumer is allowed to retry consumption.
    /// </summary>
    /// <param name="retries">Allowed retries.</param>
    /// <returns><see langword="true"/> if the consumer is allowed to retry.</returns>
    public bool CanRetry(int retries)
    {
        return Tries < retries; 
    }

    private void Try()
    {
        Tries++;
    }
}