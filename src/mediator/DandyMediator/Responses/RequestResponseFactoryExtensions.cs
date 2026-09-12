namespace DandyMediator.Responses;

/// <summary>
/// Contains response factory extension methods.
/// </summary>
public static class RequestResponseFactoryExtensions
{
    /// <summary>
    /// Creates and casts a response.
    /// </summary>
    /// <typeparam name="TResponse">Expected response type.</typeparam>
    /// <param name="factory">Response factory.</param>
    /// <param name="responseType">Response type to create.</param>
    /// <param name="args">Constructor arguments.</param>
    /// <returns>The created response.</returns>
    public static TResponse CreateAndCast<TResponse>(this IRequestResponseFactory factory, Type responseType, params object?[] args)
        where TResponse : IRequestResponse
    {
        return (TResponse)factory.Create(responseType, args);
    }
}