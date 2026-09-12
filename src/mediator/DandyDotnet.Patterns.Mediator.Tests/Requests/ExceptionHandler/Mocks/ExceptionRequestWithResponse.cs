using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.ExceptionHandler.Mocks;

internal sealed record ExceptionRequestWithResponse(CounterCallback Callback) : IRequest<bool>;

internal sealed class ExceptionRequestWithResponseExceptionHandler : IRequestExceptionHandler<ExceptionRequestWithResponse, bool>
{
    public Task HandleAsync(ExceptionRequestWithResponse exceptionRequest, Exception exception, CancellationToken cancellationToken)
    {
        exceptionRequest.Callback.Success();
        return Task.CompletedTask;
    }
}

internal sealed class ExceptionRequestWithResponseHandler : IRequestHandler<ExceptionRequestWithResponse, bool>
{
    public Task<bool> HandleAsync(ExceptionRequestWithResponse exceptionRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
