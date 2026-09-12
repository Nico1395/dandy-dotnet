using System.Collections.Concurrent;
using System.Reflection;

namespace DandyRabbitMQ.Core.Messages.Configuration;

/// <summary>
/// Contains message metadata and message discovery settings.
/// </summary>
public sealed class MessagesConfiguration
{
    /// <summary>
    /// Gets or sets message metadata indexed by runtime type.
    /// </summary>
    public required ConcurrentDictionary<Type, MessageConfiguration> MessagesByRuntimeType { get; set; }

    /// <summary>
    /// Gets or sets message metadata indexed by message key.
    /// </summary>
    public required ConcurrentDictionary<string, MessageConfiguration> MessagesByKey { get; set; }

    /// <summary>
    /// Gets or sets assemblies scanned for attributed message types.
    /// </summary>
    public required Assembly[] Assemblies { get; set; }

    /// <summary>
    /// Merges two message configurations.
    /// </summary>
    /// <param name="a">The first configuration.</param>
    /// <param name="b">The second configuration.</param>
    /// <returns>The merged configuration.</returns>
    public static MessagesConfiguration Merge(MessagesConfiguration a, MessagesConfiguration b)
    {
        // Right now we are tossing away duplicates. However, it might be more interesting in the future
        // to merge some parts of the configurations, such as the routing keys.

        var messagesByRuntimeType = a.MessagesByRuntimeType.Concat(b.MessagesByRuntimeType).DistinctBy(x => x.Key);
        var messagesByKey = a.MessagesByKey.Concat(b.MessagesByKey).DistinctBy(x => x.Key);

        return new MessagesConfiguration
        {
            MessagesByRuntimeType = new ConcurrentDictionary<Type, MessageConfiguration>(messagesByRuntimeType),
            MessagesByKey = new ConcurrentDictionary<string, MessageConfiguration>(messagesByKey),
            Assemblies = a.Assemblies.Concat(b.Assemblies).Distinct().ToArray(),
        };
    }

    /// <summary>
    /// Adds a <paramref name="message"/> configuration indexed by runtime type.
    /// </summary>
    /// <param name="message">The message configuration to add.</param>
    public void AddMessage(MessageConfiguration message)
    {
        MessagesByRuntimeType.TryAdd(message.RuntimeType, message);
    }
}
