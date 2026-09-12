using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Core.Messages.Configuration;

/// <summary>
/// Provides dependency injection registration extensions for message metadata.
/// </summary>
public static class MessageServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures message metadata.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="builderAction">An action that configures message metadata.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDandyRabbitMQMessages(this IServiceCollection services, Action<MessagesConfigurationBuilder> builderAction)
    {
        var builder = new MessagesConfigurationBuilder();
        builderAction.Invoke(builder);
        var configuration = builder.Build();

        return services.AddDandyRabbitMQMessages(configuration);
    }

    /// <summary>
    /// Adds message metadata from an existing <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="configuration">The message configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDandyRabbitMQMessages(this IServiceCollection services, MessagesConfiguration configuration)
    {
        var previous = services.BuildServiceProvider().GetService<MessagesConfiguration>();
        if (previous != null)
            configuration = MessagesConfiguration.Merge(previous, configuration);

        ScanForMessages(configuration);
        services.AddSingleton(configuration);

        return services;
    }

    private static void ScanForMessages(MessagesConfiguration configuration)
    {
        if (configuration.Assemblies.Length == 0)
            return;

        var messageTypes = configuration.Assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type.GetCustomAttributes<MessageAttribute>().Any() && type is
            {
                IsClass: true,
                IsAbstract: false,
                IsGenericTypeDefinition: false,
            })
            .ToArray();

        foreach (var messageType in messageTypes)
        {
            // In this loop we are obviously evaluating attributes. However, something to note is that we are first getting
            // or adding message configurations associated with the runtime type, since that is easy to do here. At the end,
            // when the key might be overridden by an attribute, we can then add the message configuration to the by-key cache.

            var metadata = new ConcurrentDictionary<string, object>();
            var messageConfiguration = configuration.MessagesByRuntimeType.GetOrAdd(messageType, type => new MessageConfiguration
            {
                RuntimeType = type,
                Key = type.Name,
                Metadata = metadata,
            });

            var attributes = messageType.GetCustomAttributes<MessageAttribute>().ToArray();
            foreach (var attribute in attributes)
            {
                if (attribute is MessageExchangeAttribute exchangeAttribute)
                    messageConfiguration.Exchange = exchangeAttribute.Exchange;
                else if (attribute is MessageRoutesAttribute routingKeyAttribute)
                    messageConfiguration.RoutingKeys = routingKeyAttribute.Keys;
                else if (attribute is MessageMetadataAttribute metadataAttribute)
                    metadata[metadataAttribute.Key] = metadataAttribute.Value;
                else if (attribute is MessageKeyAttribute keyAttribute)
                    messageConfiguration.Key = keyAttribute.Key;
                else
                    throw new InvalidOperationException($"Unknown attribute type: {attribute.GetType()}");
            }

            configuration.MessagesByKey[messageConfiguration.Key] = messageConfiguration;

            // If the message type name is different from the runtime type name, then we need to remove the old keyed
            // message configuration to prevent obsolete cache entries.
            if (messageConfiguration.Key != messageConfiguration.RuntimeType.Name)
                configuration.MessagesByKey.Remove(messageConfiguration.RuntimeType.Name, out _);
        }
    }
}
