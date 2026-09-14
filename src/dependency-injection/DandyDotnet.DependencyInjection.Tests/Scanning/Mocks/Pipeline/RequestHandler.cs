namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public class RequestHandler : IHandler<Request, string>
{
    public string Handle(Request request)
    {
        return "Hello world!";
    }
}