namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Describes how a consumed message should be acknowledged.
/// </summary>
public sealed class ConsumerResult
{
    private ConsumerResult() { }
    /// <summary>
    /// Gets the acknowledgement status.
    /// </summary>
    public required ConsumerStatus Status { get; init; }

    /// <summary>
    /// Gets a value indicating whether the broker should requeue the message.
    /// </summary>
    public bool Requeue { get; init; }

    /// <summary>
    /// Gets a value indicating whether the acknowledgement applies to multiple deliveries.
    /// </summary>
    public bool Multiple { get; init; }

    /// <summary>
    /// Creates an acknowledgement result for a single message.
    /// </summary>
    /// <returns>An acknowledgement result.</returns>
    public static ConsumerResult Ack()
    {
        return new ConsumerResult
        {
            Status = ConsumerStatus.Ack,
        };
    }

    /// <summary>
    /// Creates an acknowledgement result for multiple messages.
    /// </summary>
    /// <returns>An acknowledgement result with multiple-delivery handling enabled.</returns>
    public static ConsumerResult AckMultiple()
    {
        return new ConsumerResult
        {
            Status = ConsumerStatus.Ack,
            Multiple = true,
        };
    }

    /// <summary>
    /// Creates a negative acknowledgement result for a single message.
    /// </summary>
    /// <returns>A negative acknowledgement result.</returns>
    public static ConsumerResult Nack()
    {
        return new ConsumerResult
        {
            Status = ConsumerStatus.Nack,
        };
    }

    /// <summary>
    /// Creates a negative acknowledgement result that requeues the message.
    /// </summary>
    /// <returns>A requeuing negative acknowledgement result.</returns>
    public static ConsumerResult NackRequeue()
    {
        return new ConsumerResult
        {
            Status = ConsumerStatus.Nack,
            Requeue = true,
        };
    }

    /// <summary>
    /// Creates a negative acknowledgement result for multiple messages.
    /// </summary>
    /// <returns>A negative acknowledgement result with multiple-delivery handling enabled.</returns>
    public static ConsumerResult NackMultiple()
    {
        return new ConsumerResult
        {
            Status = ConsumerStatus.Ack,
            Multiple = true,
        };
    }

    /// <summary>
    /// Creates a negative acknowledgement result for multiple messages that should be requeued.
    /// </summary>
    /// <returns>A requeuing negative acknowledgement result with multiple-delivery handling enabled.</returns>
    public static ConsumerResult NackRequeueMultiple()
    {
        return new ConsumerResult
        {
            Status = ConsumerStatus.Nack,
            Requeue = true,
            Multiple = true,
        };
    }
}
