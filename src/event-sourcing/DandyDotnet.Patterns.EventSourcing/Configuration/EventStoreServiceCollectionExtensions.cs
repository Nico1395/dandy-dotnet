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
    ///         This method registers the following services with the DI container:
    ///         <list type="bullet">
    ///             <item><description><see cref="EventStoreConfiguration" /> as a singleton service.</description></item>
    ///             <item><description><see cref="IEventStore" /> implementation as a scoped service.</description></item>
    ///             <item><description><see cref="ISubscriptionManager" /> as a scoped service.</description></item>
    ///             <item><description><see cref="IEnvelopeFactory" /> as a singleton service.</description></item>
    ///             <item><description><see cref="IOutbox" /> as a scoped service.</description></item>
    ///             <item><description><see cref="AsyncOutboxDaemon" /> as a hosted service (if outbox daemon is enabled).</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         If assembly scanning is enabled via <see cref="EventStoreConfigurationBuilder.ScanInAssemblies" />,
    ///         this method also:
    ///         <list type="bullet">
    ///             <item><description>Automatically discovers and registers aggregates marked with <see cref="AggregateAttribute" />.</description></item>
    ///             <item><description>Automatically discovers and registers events marked with <see cref="EventAttribute" />.</description></item>
    ///             <item><description>Automatically discovers and registers subscribers marked with <see cref="SubscriberAttribute" />.</description></item>
    ///             <item><description>Scans for and registers implementations of <see cref="ISubscriber{TEvent}" />, <see cref="ISubscriberExceptionHandler{TEvent}" />, and <see cref="IAggregateFactory{T}" />.</description></item>
    ///         </list>
    ///     </para>
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

    /// <summary>
    ///     Discovers and registers aggregate types from the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for aggregate types.</param>
    /// <param name="configuration">The aggregates configuration to add discovered types to.</param>
    /// <remarks>
    ///     This method scans the specified assemblies for types decorated with the
    ///     <see cref="AggregateAttribute" /> and adds them to the aggregates configuration.
    /// </remarks>
    private static void AddAggregatesFromAssemblies(Assembly[] assemblies, AggregatesConfiguration configuration)
    {
        var aggregateTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetCustomAttribute<AggregateAttribute>() != null);

        foreach (var aggregateType in aggregateTypes)
            configuration.GetOrAddAggregateConfiguration(aggregateType);
    }

    /// <summary>
    ///     Discovers and registers event types from the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for event types.</param>
    /// <param name="configuration">The events configuration to add discovered types to.</param>
    /// <remarks>
    ///     This method scans the specified assemblies for types decorated with the
    ///     <see cref="EventAttribute" /> and adds them to the events configuration.
    /// </remarks>
    private static void AddEventsFromAssemblies(Assembly[] assemblies, EventsConfiguration configuration)
    {
        var eventTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetCustomAttribute<EventAttribute>() != null);

        foreach (var eventType in eventTypes)
            configuration.GetOrAddEventConfiguration(eventType);
    }

    /// <summary>
    ///     Discovers and registers subscriber types from the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for subscriber types.</param>
    /// <param name="configuration">The subscribers configuration to add discovered types to.</param>
    /// <remarks>
    ///     This method scans the specified assemblies for types decorated with the
    ///     <see cref="SubscriberAttribute" /> and adds them to the subscribers configuration.
    /// </remarks>
    private static void AddSubscribersFromAssemblies(Assembly[] assemblies, SubscribersConfiguration configuration)
    {
        var subscriberTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetCustomAttribute<SubscriberAttribute>() != null);

        foreach (var subscriberType in subscriberTypes)
            configuration.GetOrAddSubscriberConfiguration(subscriberType);
    }

    /// <summary>
    ///     Registers aggregate factories for configured aggregates.
    /// </summary>
    /// <param name="services">The service collection to register factories with.</param>
    /// <param name="configuration">The aggregates configuration containing factory configurations.</param>
    /// <remarks>
    ///     This method registers each configured aggregate factory type with the DI container,
    ///     allowing the event store to resolve and use them when reconstructing aggregates.
    /// </remarks>
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

    /// <summary>
    ///     Registers plugin services.
    /// </summary>
    /// <param name="services">The service collection to register plugin services with.</param>
    /// <param name="plugins">The dictionary of plugin configurations to process.</param>
    /// <remarks>
    ///     This method invokes the <see cref="PluginConfiguration.ConfigureServices" /> method on each
    ///     registered plugin, allowing plugins to add their own services to the container.
    /// </remarks>
    private static void AddPlugins(IServiceCollection services, IReadOnlyDictionary<string, PluginConfiguration> plugins)
    {
        foreach (var (_, configuration) in plugins)
            configuration.ConfigureServices(services);
    }
}
