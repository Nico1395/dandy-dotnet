namespace DandyDotnet.Patterns.EventSourcing;

internal interface IEnvelopeFactory
{
    Envelope Create(string streamId, object @event, long version);
}