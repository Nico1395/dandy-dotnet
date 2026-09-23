using System.Threading;
using System.Threading.Tasks;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Interface for event subscribers that handle events asynchronously.
/// </summary>
/// <typeparam name="TEvent">The type of event this subscriber handles.</typeparam>
/// <remarks>
///     <para>
///         Subscribers are components that react to events published by the event store. They implement
///         the <see cref="HandleAsync" /> method which is invoked when an event of type <typeparamref name="TEvent" />
///         is published to the outbox and ready for processing.
///     </para>
///     <para>
///         The <see cref="SubscriberContext" /> provides additional information about the event being
///         processed, including the event store instance, the envelope, retry information, and more.
///     </para>
///     <para>
///         There are two types of subscribers based on their <see cref="SubscriberMode" />:
///         <list type="bullet">
///             <item><description><see cref="SubscriberMode.Inline" />: Processed immediately during the append operation.</description></item>
///             <item><description><see cref="SubscriberMode.Async" />: Processed asynchronously by the outbox daemon.</description></item>
///         </list>
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         public class UserCreatedSubscriber : ISubscriber{UserCreatedEvent}
///         {
///             private readonly ILogger&lt;UserCreatedSubscriber&gt; _logger;
///             
///             public async Task HandleAsync(UserCreatedEvent @event, SubscriberContext context, CancellationToken cancellationToken)
///             {
///                 _logger.LogInformation("User created: {UserId}", @event.UserId);
///                 await SendWelcomeEmailAsync(@event.UserId, cancellationToken);
///             }
///         }
///     </code>
/// </example>
/// <seealso cref="ISubscriberExceptionHandler{TEvent}" />
/// <seealso cref="SubscriberAttribute" />
/// <seealso cref="SubscriberContext" />
public interface ISubscriber<in TEvent>
    where TEvent : class
{
    /// <summary>
    ///     Handles the specified event asynchronously.
    /// </summary>
    /// <param name="subscribed">
    ///     The event to handle. This is the strongly-typed event instance of type <typeparamref name="TEvent" />.
    /// </param>
    /// <param name="context">
    ///     The context containing metadata about the subscription and event being processed.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous handling operation.</returns>
    /// <remarks>
    ///     <para>
    ///         The implementation should process the event asynchronously. For long-running operations,
    ///         consider using the <paramref name="cancellationToken" /> to support cancellation.
    ///     </para>
    ///     <para>
    ///         The <paramref name="context" /> parameter provides access to:
    ///         <list type="bullet">
    ///             <item><description><see cref="SubscriberContext.EventStore" />: The event store instance.</description></item>
    ///             <item><description><see cref="SubscriberContext.Envelope" />: The envelope containing the event.</description></item>
    ///             <item><description><see cref="SubscriberContext.Mode" />: The subscriber mode (inline or async).</description></item>
    ///             <item><description><see cref="SubscriberContext.MaxRetries" />: The maximum number of retries configured.</description></item>
    ///             <item><description><see cref="SubscriberContext.RetryCount" />: The current retry count (0 for first attempt).</description></item>
    ///         </list>
    ///         The context also provides helper methods like <see cref="SubscriberContext.CanRetry" /> and
    ///         <see cref="SubscriberContext.IsFirstTry" />.
    ///     </para>
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    ///     Thrown when the operation is cancelled via the <paramref name="cancellationToken" />.
    /// </exception>
    /// <exception cref="Exception">
    ///     Any exception thrown by this method will be handled by the configured
    ///     <see cref="ISubscriberExceptionHandler{TEvent}" />, if registered.
    /// </exception>
    Task HandleAsync(TEvent subscribed, SubscriberContext context, CancellationToken cancellationToken);
}
