using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDandyMigrations(this IServiceCollection services, Action<MigrationsConfiguration> configure)
    {
        var configuration = new MigrationsConfiguration();
        configure(configuration);

        if (configuration.Driver == null)
            throw new InvalidOperationException("No persistence driver was configured.");

        configuration.Driver.ConfigureServices(services, configuration);

        if (configuration.ServiceKey == null)
            services.AddSingleton(configuration);
        else
            services.AddKeyedSingleton(configuration.ServiceKey, configuration);

        return services;
    }
}