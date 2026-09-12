using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Requests.ExceptionHandler.Mocks;

internal sealed record ExceptionRequestWithoutResponse(CounterCallback Callback) : IRequest;

internal sealed class ExceptionRequestWithoutResponseExceptionHandler : IRequestExceptionHandler<ExceptionRequestWithoutResponse>
{
    public Task HandleAsync(ExceptionRequestWithoutResponse exceptionRequest, Exception exception, CancellationToken cancellationToken)
    {
        exceptionRequest.Callback.Success();
        return Task.CompletedTask;
    }
}

internal sealed class ExceptionRequestWithoutResponseHandler : IRequestHandler<ExceptionRequestWithoutResponse>
{
    public Task HandleAsync(ExceptionRequestWithoutResponse exceptionRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
