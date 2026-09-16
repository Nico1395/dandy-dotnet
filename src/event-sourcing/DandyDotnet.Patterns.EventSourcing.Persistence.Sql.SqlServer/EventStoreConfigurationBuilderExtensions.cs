using DandyDotnet.Patterns.EventSourcing.Configuration;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

/// <summary>
///     Provides extension methods for configuring SQL Server persistence for the Event Store.
/// </summary>
public static class EventStoreConfigurationBuilderExtensions
{
    /// <summary>
    ///     Configures SQL Server as the event store persistence backend.
    /// </summary>
    /// <param name="builder">The event store configuration builder.</param>
    /// <param name="builderAction">An optional action to configure SQL Server-specific settings such as connection string.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public static EventStoreConfigurationBuilder UseSqlServer(this EventStoreConfigurationBuilder builder, Action<SqlServerConfigurationBuilder>? builderAction = null)
    {
        var configurationBuilder = new SqlServerConfigurationBuilder();
        builderAction?.Invoke(configurationBuilder);
        var configuration = configurationBuilder.Build();

        return builder.UsePlugin(configuration);
    }
}