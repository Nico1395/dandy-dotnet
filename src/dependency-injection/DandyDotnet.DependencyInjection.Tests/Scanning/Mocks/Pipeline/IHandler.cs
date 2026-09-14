namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public interface IHandler<in T, out TResult>
    where T : class, IHandlerRequest<TResult>
{
    TResult Handle(T request);
}