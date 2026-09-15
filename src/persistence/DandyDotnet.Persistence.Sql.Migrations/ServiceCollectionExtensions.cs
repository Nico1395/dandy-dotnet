using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMigrations(this IServiceCollection services, Action<MigrationsConfiguration> configure)
    {
        var configuration = new MigrationsConfiguration();
        configure(configuration);

        if (configuration.Driver == null)
            throw new InvalidOperationException("No persistence driver was configured.");

        configuration.Driver.ConfigureServices(services, configuration);

        services.AddKeyedSingletonOrDefault<IMigrationRunner, MigrationRunner>(configuration.ServiceKey, (sp, _) => new MigrationRunner(configuration, sp));
        services.AddKeyedSingletonOrDefault(configuration.ServiceKey, configuration);

        services.ScanAndAdd(scanner =>
        {
            scanner.ScanIn(configuration.Assemblies);
            scanner.ScanFor<IMigration>(migration => migration.WithKey(configuration.ServiceKey));
        });

        if (configuration.ServiceKey == null)
            services.AddTransientRange<IMigration>(configuration.MigrationTypes);
        else
            services.AddKeyedTransientRange<IMigration>(configuration.ServiceKey, configuration.MigrationTypes);

        return services;
    }
}