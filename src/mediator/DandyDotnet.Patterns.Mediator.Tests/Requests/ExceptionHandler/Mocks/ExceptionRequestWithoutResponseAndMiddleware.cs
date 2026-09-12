using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.ExceptionHandler.Mocks;

internal sealed record ExceptionRequestWithoutResponseAndMiddleware(CounterCallback Callback) : IRequest;

internal sealed class ExceptionRequestWithoutResponseAndMiddlewareExceptionHandler : IRequestExceptionHandler<ExceptionRequestWithoutResponseAndMiddleware>
{
    public Task HandleAsync(ExceptionRequestWithoutResponseAndMiddleware exceptionRequest, Exception exception, CancellationToken cancellationToken)
    {
        exceptionRequest.Callback.Success();
        return Task.CompletedTask;
    }
}

internal sealed class ExceptionRequestWithoutResponseAndMiddlewareMiddleware : IRequestMiddleware<ExceptionRequestWithoutResponseAndMiddleware>
{
    public Task InterceptAsync(ExceptionRequestWithoutResponseAndMiddleware exceptionRequest, RequestHandlerDelegate nextStep, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

internal sealed class ExceptionRequestWithoutResponseAndMiddlewareHandler : IRequestHandler<ExceptionRequestWithoutResponseAndMiddleware>
{
    public Task HandleAsync(ExceptionRequestWithoutResponseAndMiddleware exceptionRequest, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}