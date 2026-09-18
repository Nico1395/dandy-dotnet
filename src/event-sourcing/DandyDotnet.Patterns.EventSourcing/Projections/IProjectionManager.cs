using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Outbox;

namespace DandyDotnet.Patterns.EventSourcing.Projections;

public interface IProjectionManager
{
    Task ProjectAsync(OutboxEnvelope outboxEnvelope, ProjectionMode[] modes, CancellationToken cancellationToken);
}