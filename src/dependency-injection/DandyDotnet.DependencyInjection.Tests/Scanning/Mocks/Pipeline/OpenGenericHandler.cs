namespace DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;

public interface IOpenGenericHandler<T>;

public class OpenGenericHandler<T> : IOpenGenericHandler<T>;

public class ClosedGenericHandler : IOpenGenericHandler<Request>;
