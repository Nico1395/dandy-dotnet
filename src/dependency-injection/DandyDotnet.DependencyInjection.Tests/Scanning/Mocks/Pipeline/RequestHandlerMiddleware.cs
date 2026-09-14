namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public class RequestHandlerMiddleware : IHandlerMiddleware<Request, string>
{
    public string Handle(Request request, Func<Request, string> next)
    {
        return next(request);
    }
}