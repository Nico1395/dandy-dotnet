using DandyDotnet.Patterns.EventSourcing.Configuration;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;

public static class EventStoreConfigurationBuilderExtensions
{
    public static EventStoreConfigurationBuilder UseNpgsql(this EventStoreConfigurationBuilder builder, Action<NpgsqlConfigurationBuilder>? builderAction = null)
    {
        var configurationBuilder = new NpgsqlConfigurationBuilder();
        builderAction?.Invoke(configurationBuilder);
        var configuration = configurationBuilder.Build();

        return builder.UsePlugin(configuration);
    }
}