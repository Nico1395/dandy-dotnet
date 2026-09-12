using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Requests.ExceptionHandler.Mocks;

internal sealed record ExceptionRequestWithResponseAndMiddleware(CounterCallback Callback) : IRequest<bool>;

internal sealed class ExceptionRequestWithResponseAndMiddlewareExceptionHandler : IRequestExceptionHandler<ExceptionRequestWithResponseAndMiddleware, bool>
{
    public Task HandleAsync(ExceptionRequestWithResponseAndMiddleware exceptionRequest, Exception exception, CancellationToken cancellationToken)
    {
        exceptionRequest.Callback.Success();
        return Task.CompletedTask;
    }
}

internal sealed class ExceptionRequestWithResponseAndMiddlewareMiddleware : IRequestMiddleware<ExceptionRequestWithResponseAndMiddleware, bool>
{
    public Task<bool> InterceptAsync(ExceptionRequestWithResponseAndMiddleware exceptionRequest, RequestHandlerDelegate<bool> nextStep, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

internal sealed class ExceptionRequestWithResponseAndMiddlewareHandler : IRequestHandler<ExceptionRequestWithResponseAndMiddleware, bool>
{
    public Task<bool> HandleAsync(ExceptionRequestWithResponseAndMiddleware exceptionRequest, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
