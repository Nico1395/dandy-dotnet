using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;

namespace DandyDotnet.Patterns.EventSourcing.Persistence;

public interface IProjectionRepository
{
    Task<ProjectionEnvelopeEntity?> GetProjectionByKeyAsync(string streamId, string key, CancellationToken cancellationToken);
    Task InsertAsync(ProjectionEnvelopeEntity projectionEntity, CancellationToken cancellationToken);
    Task UpdateAsync(ProjectionEnvelopeEntity projectionEntity, CancellationToken cancellationToken);
}