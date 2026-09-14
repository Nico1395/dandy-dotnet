namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public interface IHandlerExceptionHandler<in T, in TResult>
    where T : class, IHandlerRequest<TResult>
{
    void Handle(T request, Exception exception);
}