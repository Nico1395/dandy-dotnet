using System.Collections.Concurrent;

namespace DandyMediator.Responses;

/// <summary>
/// Creates concrete response instances.
/// </summary>
public class RequestResponseFactory : IRequestResponseFactory
{
    private static readonly ConcurrentDictionary<Type, bool> _typeIsResponseCache = [];

    /// <summary>
    /// Creates a response instance.
    /// </summary>
    /// <param name="responseType">Response type to create.</param>
    /// <param name="args">Constructor arguments.</param>
    /// <returns>The created response.</returns>
    public virtual object Create(Type responseType, params object?[] args)
    {
        if (!TypeIsResponse(responseType))
            throw new ArgumentException($"Type {responseType} is not a response type.");

        return Activator.CreateInstance(responseType, args) ?? throw new InvalidOperationException($"Failed to create instance of {responseType}.");
    }

    /// <summary>
    /// Determines whether a type is a response type.
    /// </summary>
    /// <param name="type">The type to check for.</param>
    /// <returns><see langword="true"/> if the <paramref name="type"/> is a response type, <see langword="false"/> if not.</returns>
    protected virtual bool TypeIsResponse(Type type)
    {
        return _typeIsResponseCache.GetOrAdd(type, t => t.IsAssignableTo(typeof(IRequestResponse)));
    }
}