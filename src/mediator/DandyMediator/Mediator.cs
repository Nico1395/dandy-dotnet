using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DandyMediator;

internal sealed class Mediator(
    IRequestPipeline requestPipeline,
    IServiceProvider serviceProvider) : IMediator
{
    private readonly ConcurrentDictionary<Type, MethodInfo> _executeAsync = [];

    public Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var notificationHandlers = serviceProvider.GetServices<INotificationHandler<TNotification>>();
        var handlerTasks = notificationHandlers.Select(notificationHandler => HandleNotificationAsync(notification, notificationHandler, cancellationToken));

        return Task.WhenAll(handlerTasks);
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var executeAsync = _executeAsync.GetOrAdd(request.GetType(), requestType =>
        {
            var markupInterface = requestType.GetInterfaces().Single(i => i.GetGenericTypeDefinition() == typeof(IRequest<>));
            var responseType = markupInterface.GetGenericArguments()[0];

            return typeof(IRequestPipeline)
                .GetMethods()
                .Single(methodInfo =>
                    methodInfo.Name == nameof(IRequestPipeline.ExecuteAsync) &&
                    methodInfo.GetGenericArguments().Length == 2)
                .MakeGenericMethod(requestType, responseType);
        });

        var responseTask = executeAsync.Invoke(requestPipeline, [request, cancellationToken]) as Task<TResponse>;
        return responseTask ?? throw new UnreachableException($"Could not execute request of type {request.GetType().FullName}");
    }

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var executeAsync = _executeAsync.GetOrAdd(request.GetType(), requestType =>
        {
            return typeof(IRequestPipeline)
                .GetMethods()
                .Single(methodInfo =>
                    methodInfo.Name == nameof(IRequestPipeline.ExecuteAsync) &&
                    methodInfo.GetGenericArguments().Length == 1)
                .MakeGenericMethod(requestType);
        });

        var task = executeAsync.Invoke(requestPipeline, [request, cancellationToken]) as Task;
        return task ?? throw new UnreachableException($"Could not execute request of type {request.GetType().FullName}");
    }

    private async Task HandleNotificationAsync<TNotification>(TNotification notification, INotificationHandler<TNotification> notificationHandler, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        try
        {
            await notificationHandler.HandleAsync(notification, cancellationToken);
        }
        catch (Exception ex)
        {
            var exceptionHandler = serviceProvider.GetService<INotificationExceptionHandler<TNotification>>();
            if (exceptionHandler != null)
                await exceptionHandler.HandleAsync(notification, ex, cancellationToken);

            throw;
        }
    }
}
