using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Outbox;
using DandyDotnet.Patterns.EventSourcing.Persistence;

namespace DandyDotnet.Patterns.EventSourcing.Projections;

internal sealed class ProjectionManager(
    EventStoreConfiguration eventStoreConfiguration,
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : IProjectionManager
{
    public Task ProjectAsync(OutboxEnvelope outboxEnvelope, ProjectionMode[] modes, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}