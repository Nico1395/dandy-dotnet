namespace DandyMediator.Responses;

/// <summary>
/// Creates response instances.
/// </summary>
public interface IRequestResponseFactory
{
    /// <summary>
    /// Creates a response instance.
    /// </summary>
    /// <param name="responseType">Response type to create.</param>
    /// <param name="args">Constructor arguments.</param>
    /// <returns>The created response.</returns>
    object Create(Type responseType, params object?[] args);
}