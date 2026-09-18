namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
/// Exception thrown when an event is not handled by an aggregate during event sourcing operations.
/// </summary>
/// <param name="message">Message describing the unhandled event scenario.</param>
/// <remarks>
/// This exception is thrown when an aggregate does not have a handler for a specific event.
/// The message includes details about the event type and aggregate type involved.
/// </remarks>
/// <seealso cref="EventSourcingException"/>
public sealed class UnhandledEventException(string message) : EventSourcingException(message)
{
    /// <summary>
    /// Throws an <see cref="UnhandledEventException"/> when an event is not handled by an aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate object that does not handle the event.</param>
    /// <param name="event">The event object that is not handled by the aggregate.</param>
    /// <exception cref="UnhandledEventException">Thrown when the event is not handled by the aggregate.</exception>
    public static void Throw(object aggregate, object @event)
    {
        throw new UnhandledEventException($"Event of type '{@event.GetType()}' is not being handled by aggregate of type '{aggregate.GetType()}'.");
    }

    /// <summary>
    /// Throws an <see cref="UnhandledEventException"/> when an event is not handled by an aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate object that does not handle the event.</param>
    /// <param name="envelope">The envelope containing the event that is not handled by the aggregate.</param>
    /// <exception cref="UnhandledEventException">Thrown when the event is not handled by the aggregate.</exception>
    public static void Throw(object aggregate, IReadOnlyEnvelope envelope)
    {
        throw new UnhandledEventException($"Event with key '{envelope.EventKey}' and version '{envelope.Version}' is not being handled by aggregate of type '{aggregate.GetType()}'.");
    }
}