namespace DandyDotnet.Patterns.EventSourcing;

public interface IEnvelopeFactory
{
    Envelope Create(string streamId, object @event, long version);
}