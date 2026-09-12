namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IAggregateFactory<TAggregate>
    where TAggregate : class
{
    TAggregate Create(TAggregate? snapshot, IReadOnlyEnvelope[] envelopes);
}