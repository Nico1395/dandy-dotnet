namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
/// Thrown during aggregate-related operations.
/// </summary>
/// <param name="message">The exception message.</param>
public sealed class AggregateException(string message) : EventSourcingException(message);