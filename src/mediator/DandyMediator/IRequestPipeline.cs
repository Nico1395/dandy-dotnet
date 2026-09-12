namespace DandyMediator;

internal interface IRequestPipeline
{
    Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest;
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse>;
}
