namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
/// Thrown during event stream related operations.
/// </summary>
public sealed class EventStreamException : EventSourcingException
{
    /// <summary>
    /// Creates an exception with the given <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The exception's message.</param>
    public EventStreamException(string message) : base(message)
    {
    }

    /// <summary>
    /// Checks whether any of the <paramref name="tags"/> contain the tag delimiter.
    /// </summary>
    /// <param name="tags">The tags to be checked.</param>
    /// <exception cref="EventStreamException">Thrown if the <paramref name="tags"/> contain delimiter.</exception>
    public static void ThrowIfTagsContainDelimiter(string[] tags)
    {
        if (!tags.Any(t => t.Contains(';')))
            return;

        throw new EventStreamException($"Tags '{string.Join(", ", tags)}' contain the delimiter ';'.");
    }
}