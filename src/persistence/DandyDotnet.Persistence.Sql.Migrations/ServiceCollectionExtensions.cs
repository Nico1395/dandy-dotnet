using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
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
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IMigrationRunner, MigrationRunner>();
        }
        else
        {
            services.AddKeyedSingleton(configuration.ServiceKey, configuration);
            services.AddKeyedSingleton<IMigrationRunner, MigrationRunner>(configuration.ServiceKey);
        }

        AddMigrations(services, configuration);
        return services;
    }

    private static void AddMigrations(IServiceCollection services, MigrationsConfiguration configuration)
    {
        var migrationTypes = configuration.Assemblies
            .SelectMany(a => a
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(IMigration))))
            .Concat(configuration.MigrationTypes)
            .Distinct();

        foreach (var migrationType in migrationTypes)
            services.AddTransient(typeof(IMigration), migrationType);
    }
}