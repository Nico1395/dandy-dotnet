namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public interface IHandlerMiddleware<T, TResult>
    where T : class, IHandlerRequest<TResult>
{
    TResult Handle(T request, Func<T, TResult> next);
}