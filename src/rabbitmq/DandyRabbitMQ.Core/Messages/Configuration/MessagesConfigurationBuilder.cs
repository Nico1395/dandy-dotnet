using System.Collections.Concurrent;
using System.Reflection;

namespace DandyRabbitMQ.Core.Messages.Configuration;

/// <summary>
/// Builds message metadata and discovery configuration.
/// </summary>
public sealed class MessagesConfigurationBuilder
{
    private readonly Dictionary<Type, MessageConfiguration> _messagesByRuntimeType = new();
    private readonly Dictionary<string, MessageConfiguration> _messagesByKey = new();
    private Assembly[] _assemblies = [];
    /// <summary>
    /// Initializes an empty message configuration builder.
    /// </summary>
    public MessagesConfigurationBuilder()
    {
    }

    /// <summary>
    /// Initializes a builder from an existing <paramref name="configuration"/>.
    /// </summary>
    /// <param name="configuration">The configuration to copy, or <see langword="null"/>.</param>
    public MessagesConfigurationBuilder(MessagesConfiguration? configuration)
    {
        if (configuration == null)
            return;

        _messagesByRuntimeType = new Dictionary<Type, MessageConfiguration>(configuration.MessagesByRuntimeType);
        _assemblies = configuration.Assemblies;
    }

    /// <summary>
    /// Adds and configures a message type.
    /// </summary>
    /// <param name="messageType">The message runtime type.</param>
    /// <param name="builderAction">An action that configures the message.</param>
    /// <returns>This builder.</returns>
    public MessagesConfigurationBuilder AddMessage(Type messageType, Action<MessageConfigurationBuilder> builderAction)
    {
        var builder = new MessageConfigurationBuilder(messageType);
        builderAction.Invoke(builder);
        var message = builder.Build();

        _messagesByRuntimeType[message.RuntimeType] = message;
        _messagesByKey[message.Key] = message;

        return this;
    }

    /// <summary>
    /// Sets the <paramref name="assemblies"/> scanned for attributed message types.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>This builder.</returns>
    public MessagesConfigurationBuilder ScanInAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
        return this;
    }

    /// <summary>
    /// Builds the messages configuration.
    /// </summary>
    /// <returns>The messages configuration.</returns>
    public MessagesConfiguration Build()
    {
        return new MessagesConfiguration
        {
            MessagesByRuntimeType = new ConcurrentDictionary<Type, MessageConfiguration>(_messagesByRuntimeType),
            MessagesByKey = new ConcurrentDictionary<string, MessageConfiguration>(_messagesByKey),
            Assemblies = _assemblies,
        };
    }
}
