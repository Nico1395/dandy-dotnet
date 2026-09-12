using Microsoft.Extensions.DependencyInjection;

namespace DandyMediator;

internal sealed class RequestPipeline(IServiceProvider serviceProvider) : IRequestPipeline
{
    public async Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        try
        {
            var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
            var middlewares = serviceProvider.GetServices<IRequestMiddleware<TRequest>>();

            RequestHandlerDelegate handlerDelegate = () => requestHandler.HandleAsync(request, cancellationToken);

            foreach (var middleware in middlewares.Reverse())
            {
                var next = handlerDelegate;
                handlerDelegate = () => middleware.InterceptAsync(request, next, cancellationToken);
            }

            await handlerDelegate.Invoke();
        }
        catch (Exception ex)
        {
            var exceptionHandler = serviceProvider.GetService<IRequestExceptionHandler<TRequest>>();
            if (exceptionHandler != null)
                await exceptionHandler.HandleAsync(request, ex, cancellationToken);

            throw;
        }
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        try
        {
            var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
            var middlewares = serviceProvider.GetServices<IRequestMiddleware<TRequest, TResponse>>();

            RequestHandlerDelegate<TResponse> handlerDelegate = () => requestHandler.HandleAsync(request, cancellationToken);

            foreach (var middleware in middlewares.Reverse())
            {
                var next = handlerDelegate;
                handlerDelegate = () => middleware.InterceptAsync(request, next, cancellationToken);
            }

            return await handlerDelegate.Invoke();
        }
        catch (Exception ex)
        {
            var exceptionHandler = serviceProvider.GetService<IRequestExceptionHandler<TRequest, TResponse>>();
            if (exceptionHandler != null)
                await exceptionHandler.HandleAsync(request, ex, cancellationToken);

            throw;
        }
    }
}
