using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Consumer.Worker;

/// <summary>
/// Executes consumers and middleware resolved from dependency injection.
/// </summary>
public class ConsumerPipeline(IServiceProvider serviceProvider) : IConsumerPipeline
{
    /// <inheritdoc/>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="message">The message to process.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="cancellationToken">The token used to cancel processing.</param>
    /// <returns>The acknowledgement result.</returns>
    public async Task<ConsumerResult> ExecuteAsync<TMessage>(TMessage message, ConsumerContext context, CancellationToken cancellationToken)
    {
        try
        {
            var consumer = serviceProvider.GetRequiredService<IConsumer<TMessage>>();
            var middlewares = serviceProvider.GetServices<IConsumerMiddleware<TMessage>>();

            ConsumerDelegate consumerDelegate = () => consumer.ConsumeAsync(message, context, cancellationToken);

            foreach (var middleware in middlewares.Reverse())
            {
                var next = consumerDelegate;
                consumerDelegate = () => middleware.InterceptAsync(message, context, next, cancellationToken);
            }

            return await consumerDelegate.Invoke();
        }
        catch (Exception exception)
        {
            var exceptionHandler = serviceProvider.GetService<IConsumerExceptionHandler<TMessage>>();
            if (exceptionHandler != null)
                await exceptionHandler.HandleAsync(message, context, exception, cancellationToken);

            throw;
        }
    }
}
