using DandyDotnet.Patterns.EventSourcing.Configuration;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

public static class EventStoreConfigurationBuilderExtensions
{
    public static EventStoreConfigurationBuilder UseSqlServer(this EventStoreConfigurationBuilder builder, Action<SqlServerConfigurationBuilder>? builderAction = null)
    {
        var configurationBuilder = new SqlServerConfigurationBuilder();
        builderAction?.Invoke(configurationBuilder);
        var configuration = configurationBuilder.Build();

        return builder.UsePlugin(configuration);
    }
}