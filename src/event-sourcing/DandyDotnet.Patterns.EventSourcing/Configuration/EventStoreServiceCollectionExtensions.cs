using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;
using DandyDotnet.Patterns.EventSourcing.Configuration.Events;
using DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;
using DandyDotnet.Patterns.EventSourcing.Outbox;
using DandyDotnet.Patterns.EventSourcing.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

public static class EventStoreServiceCollectionExtensions
{
    public static IServiceCollection AddDandyEventSourcing(this IServiceCollection services, Action<EventStoreConfigurationBuilder> builderAction)
    {
        var builder = new EventStoreConfigurationBuilder();
        builderAction(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<ISubscriptionManager, SubscriptionManager>();
        services.AddSingleton<IEnvelopeFactory, EnvelopeFactory>();
        services.AddScoped<IOutbox, EventSourcing.Outbox.Outbox>();

        if (configuration.Outbox.DaemonEnabled)
            services.AddHostedService<AsyncOutboxDaemon>();

        if (configuration.Assemblies != null)
        {
            AddAggregatesFromAssemblies(configuration.Assemblies, configuration.Aggregates);
            AddEventsFromAssemblies(configuration.Assemblies, configuration.Events);
            AddSubscribersFromAssemblies(configuration.Assemblies, configuration.Subscribers);

            services.ScanAndAdd(scanner =>
            {
                scanner.ScanIn(configuration.Assemblies);
                scanner.ScanFor(typeof(ISubscriber<>));
                scanner.ScanFor(typeof(ISubscriberExceptionHandler<>), handler =>
                {
                    handler.AllowOpenGeneric();
                });
                scanner.ScanFor(typeof(IAggregateFactory<>));
                scanner.Build();
            });
        }

        foreach (var aggregateConfiguration in configuration.Aggregates.AggregatesByType.Values)
        {
            if (aggregateConfiguration.FactoryType == null)
                continue;

            services.AddTransient(
                typeof(IAggregateFactory<>).MakeGenericType(aggregateConfiguration.RuntimeType),
                aggregateConfiguration.FactoryType);
        }

        AddAggregateFactories(services, configuration.Aggregates);
        AddPlugins(services, configuration.Plugins);

        return services;
    }

    private static void AddAggregatesFromAssemblies(Assembly[] assemblies, AggregatesConfiguration configuration)
    {
        var aggregateTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetCustomAttribute<AggregateAttribute>() != null);

        foreach (var aggregateType in aggregateTypes)
            configuration.GetOrAddAggregateConfiguration(aggregateType);
    }

    private static void AddEventsFromAssemblies(Assembly[] assemblies, EventsConfiguration configuration)
    {
        var eventTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetCustomAttribute<EventAttribute>() != null);
        
        foreach (var eventType in eventTypes)
            configuration.GetOrAddEventConfiguration(eventType);
    }

    private static void AddSubscribersFromAssemblies(Assembly[] assemblies, SubscribersConfiguration configuration)
    {
        var subscriberTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetCustomAttribute<SubscriberAttribute>() != null);
        
        foreach (var subscriberType in subscriberTypes)
            configuration.GetOrAddSubscriberConfiguration(subscriberType);
    }

    private static void AddAggregateFactories(IServiceCollection services, AggregatesConfiguration configuration)
    {
        foreach (var aggregateConfiguration in configuration.AggregatesByType.Values)
        {
            if (aggregateConfiguration.FactoryType == null)
                continue;

            services.AddTransient(
                typeof(IAggregateFactory<>).MakeGenericType(aggregateConfiguration.RuntimeType),
                aggregateConfiguration.FactoryType);
        }
    }

    private static void AddPlugins(IServiceCollection services, IReadOnlyDictionary<string, PluginConfiguration> plugins)
    {
        foreach (var (_, configuration) in plugins)
            configuration.ConfigureServices(services);
    }
}