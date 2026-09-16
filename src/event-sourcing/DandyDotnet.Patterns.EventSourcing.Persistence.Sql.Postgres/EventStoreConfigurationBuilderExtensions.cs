using DandyDotnet.Patterns.EventSourcing.Configuration;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;

/// <summary>
///     Provides extension methods for configuring PostgreSQL persistence for the Event Store.
/// </summary>
public static class EventStoreConfigurationBuilderExtensions
{
    /// <summary>
    ///     Configures PostgreSQL as the event store persistence backend.
    /// </summary>
    /// <param name="builder">The event store configuration builder.</param>
    /// <param name="builderAction">An optional action to configure PostgreSQL-specific settings such as connection string.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public static EventStoreConfigurationBuilder UseNpgsql(this EventStoreConfigurationBuilder builder, Action<NpgsqlConfigurationBuilder>? builderAction = null)
    {
        var configurationBuilder = new NpgsqlConfigurationBuilder();
        builderAction?.Invoke(configurationBuilder);
        var configuration = configurationBuilder.Build();

        return builder.UsePlugin(configuration);
    }
}