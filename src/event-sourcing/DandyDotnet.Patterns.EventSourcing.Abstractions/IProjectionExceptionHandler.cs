namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface IProjectionExceptionHandler<in TProjection>
    where TProjection : class
{
    Task HandleAsync(TProjection projection, Exception exception, CancellationToken cancellationToken);
}