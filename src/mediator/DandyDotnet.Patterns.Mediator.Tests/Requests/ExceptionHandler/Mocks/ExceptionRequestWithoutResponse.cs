using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.ExceptionHandler.Mocks;

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
