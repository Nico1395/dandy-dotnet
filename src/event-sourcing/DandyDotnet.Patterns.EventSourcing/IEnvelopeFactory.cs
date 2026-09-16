namespace DandyDotnet.Patterns.EventSourcing;

/// <summary>
///     Defines a factory interface for creating event envelopes.
/// </summary>
internal interface IEnvelopeFactory
{
    /// <summary>
    ///     Creates a new <see cref="Envelope" /> wrapping the given event data and stream version.
    /// </summary>
    /// <param name="streamId">The unique identifier of the event stream.</param>
    /// <param name="event">The domain event instance to wrap.</param>
    /// <param name="version">The sequence version number for this event in the stream.</param>
    /// <returns>A new <see cref="Envelope" /> instance.</returns>
    Envelope Create(string streamId, object @event, long version);
}