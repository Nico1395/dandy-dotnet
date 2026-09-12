namespace DandyRabbitMQ.Consumer.Interceptors;

/// <summary>
/// Provides no-op hooks for consumer acknowledgement interception.
/// </summary>
public abstract class ConsumerInterceptor : IConsumerInterceptor
{
    /// <inheritdoc/>
    public Task OnAfterAckAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnAfterNackAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
