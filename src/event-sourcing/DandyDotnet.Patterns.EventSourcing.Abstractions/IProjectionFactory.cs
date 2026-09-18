namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IProjectionFactory<TProjection>
    where TProjection : class
{
    TProjection Create(TProjection? projection, IReadOnlyEnvelope[] envelopes);
}