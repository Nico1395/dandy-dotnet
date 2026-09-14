namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public class RequestExceptionHandler : IHandlerExceptionHandler<Request, string>
{
    public void Handle(Request request, Exception exception)
    {
        Console.WriteLine($"Exception occurred in handler: {request.GetType().Name}");
    }
}