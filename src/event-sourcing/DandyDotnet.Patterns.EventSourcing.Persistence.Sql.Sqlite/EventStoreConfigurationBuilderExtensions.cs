using DandyDotnet.Patterns.EventSourcing.Configuration;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;

/// <summary>
///     Provides extension methods for configuring SQLite persistence for the Event Store.
/// </summary>
public static class EventStoreConfigurationBuilderExtensions
{
    /// <summary>
    ///     Configures SQLite as the event store persistence backend.
    /// </summary>
    /// <param name="builder">The event store configuration builder.</param>
    /// <param name="builderAction">An optional action to configure SQLite-specific settings such as connection string.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public static EventStoreConfigurationBuilder UseSqlite(this EventStoreConfigurationBuilder builder, Action<SqliteConfigurationBuilder>? builderAction = null)
    {
        var configurationBuilder = new SqliteConfigurationBuilder();
        builderAction?.Invoke(configurationBuilder);
        var configuration = configurationBuilder.Build();

        return builder.UsePlugin(configuration);
    }
}