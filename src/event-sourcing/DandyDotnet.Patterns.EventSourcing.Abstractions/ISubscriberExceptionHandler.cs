using System;
using System.Threading;
using System.Threading.Tasks;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Interface for exception handlers that process failures in event subscribers.
/// </summary>
/// <typeparam name="TEvent">The type of event being processed when the exception occurred.</typeparam>
/// <remarks>
///     <para>
///         Exception handlers are invoked when a subscriber throws an exception during event processing.
///         They allow for centralized error handling, logging, and potentially retrying the failed operation.
///     </para>
///     <para>
///         Exception handlers can be registered during event store configuration and are called before
///         any retry logic is applied. They can inspect the exception, the event, and the subscriber context
///         to determine the appropriate action.
///     </para>
///     <para>
///         The <see cref="EventStoreConfigurationBuilder.OnSubscriberException" /> property can be used to
///         register a global exception handler that applies to all subscribers.
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         public class LoggingExceptionHandler : ISubscriberExceptionHandler&lt;UserCreatedEvent&gt;
///         {
///             private readonly ILogger&lt;LoggingExceptionHandler&gt; _logger;
///             
///             public async Task HandleAsync(UserCreatedEvent @event, SubscriberContext context, Exception exception, CancellationToken cancellationToken)
///             {
///                 _logger.LogError(exception, "Failed to process UserCreatedEvent for stream {StreamId}", context.Envelope.StreamId);
///                 
///                 if (context.CanRetry(3))
///                 {
///                     _logger.LogInformation("Will retry processing. Attempt {RetryCount}", context.RetryCount + 1);
///                 }
///             }
///         }
///     </code>
/// </example>
/// <seealso cref="ISubscriber{TEvent}" />
/// <seealso cref="EventStoreConfigurationBuilder.OnSubscriberException" />
public interface ISubscriberExceptionHandler<in TEvent>
    where TEvent : class
{
    /// <summary>
    ///     Handles an exception that occurred during subscriber processing.
    /// </summary>
    /// <param name="@event">
    ///     The event that was being processed when the exception occurred.
    /// </param>
    /// <param name="context">
    ///     The context containing metadata about the subscription and event being processed.
    /// </param>
    /// <param name="exception">
    ///     The exception that was thrown during subscriber processing.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous exception handling operation.</returns>
    /// <remarks>
    ///     <para>
    ///         The implementation can inspect the <paramref name="exception" />, <paramref name="@event" />,
    ///         and <paramref name="context" /> to determine the appropriate response.
    ///     </para>
    ///     <para>
    ///         This method is called before any retry logic. The handler can choose to:
    ///         <list type="bullet">
    ///             <item><description>Log the exception for later analysis.</description></item>
    ///             <item><description>Attempt to recover from the exception.</description></item>
    ///             <item><description>Throw a new exception to fail the processing completely.</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         The <paramref name="context" /> contains information about the retry state, including
    ///         <see cref="SubscriberContext.RetryCount" /> and <see cref="SubscriberContext.MaxRetries" />,
    ///         which can be used to implement custom retry logic.
    ///     </para>
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    ///     Thrown when the operation is cancelled via the <paramref name="cancellationToken" />.
    /// </exception>
    Task HandleAsync(TEvent @event, SubscriberContext context, Exception exception, CancellationToken cancellationToken);
}
