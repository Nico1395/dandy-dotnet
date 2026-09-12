namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IEnvelopeFactory
{
    Envelope Create(string streamId, object @event, long version);
}