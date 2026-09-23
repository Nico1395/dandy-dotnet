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

/// <summary>
///     Extension methods for <see cref="IServiceCollection" /> to configure Event Sourcing services.
/// </summary>
/// <remarks>
///     <para>
///         These extensions provide the entry point for configuring and registering all Event Sourcing
///         components with the dependency injection container.
///     </para>
///     <para>
///         The <see cref="AddEventSourcing" /> method registers the core event store services, configures
///         aggregate types, events, subscribers, and optional assembly scanning for automatic discovery.
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         // Basic Event Sourcing setup
///         services.AddEventSourcing(builder => builder
///             .Aggregates.AddAggregate&lt;UserAggregate&gt;(aggregate => aggregate
///                 .WithKey("User")
///                 .UseSnapshots(10)));
///         
///         // With assembly scanning for automatic discovery
///         services.AddEventSourcing(builder => builder
///             .ScanInAssemblies(Assembly.GetExecutingAssembly()));
///     </code>
/// </example>
public static class EventStoreServiceCollectionExtensions
{
    /// <summary>
    ///     Registers Event Sourcing services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="builderAction">
    ///     An action that configures the <see cref="EventStoreConfigurationBuilder" /> to customize
    ///     the Event Sourcing setup.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         Custom aggregate factories configured with a specific factory type are also registered
    ///         with the DI container.
    ///     </para>
    ///     <para>
    ///         Plugin configurations are processed and their services are registered via
    ///         <see cref="PluginConfiguration.ConfigureServices" />.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="services" /> or <paramref name="builderAction" /> is <see langword="null" />.
    /// </exception>
    /// <seealso cref="EventStoreConfigurationBuilder" />
    /// <seealso cref="EventStoreConfiguration" />
    public static IServiceCollection AddEventSourcing(this IServiceCollection services, Action<EventStoreConfigurationBuilder> builderAction)
    {
        var builder = new EventStoreConfigurationBuilder();
        builderAction(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<ISubscriptionManager, SubscriptionManager>();
        services.AddScoped<IOutbox, EventSourcing.Outbox.Outbox>();

        if (configuration.Outbox.DaemonEnabled)
            services.AddHostedService<AsyncOutboxDaemon>();

        if (configuration.Assemblies != null)
        {
            var types = configuration.Assemblies.SelectMany(a => a.GetTypes()).ToArray();

            AddAggregatesFromAssemblies(types, configuration.Aggregates);
            AddEventsFromAssemblies(types, configuration.Events);
            AddSubscribersFromAssemblies(types, configuration.Subscribers);

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

    private static void AddAggregatesFromAssemblies(Type[] types, AggregatesConfiguration configuration)
    {
        foreach (var aggregateType in types.Where(t => t.GetCustomAttribute<AggregateAttribute>() != null))
            configuration.GetOrAddAggregateConfiguration(aggregateType);
    }

    private static void AddEventsFromAssemblies(Type[] types, EventsConfiguration configuration)
    {
        foreach (var eventType in types.Where(t => t.GetCustomAttribute<EventAttribute>() != null))
            configuration.GetOrAddEventConfiguration(eventType);
    }

    private static void AddSubscribersFromAssemblies(Type[] types, SubscribersConfiguration configuration)
    {
        foreach (var subscriberType in types.Where(t => t.GetCustomAttribute<SubscriberAttribute>() != null))
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
