namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
/// Exception thrown during event sourcing processes.
/// </summary>
/// <param name="message">Message of the exception.</param>
public abstract class EventSourcingException(string message) : Exception(message);