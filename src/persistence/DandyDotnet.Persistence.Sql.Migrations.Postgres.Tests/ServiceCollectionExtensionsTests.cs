using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDandyMigrations_WithoutDriver_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => new ServiceCollection().AddMigrations(cfg =>
        {
            cfg.AddMigration<Migration1>();
        }));
    }
    
    [Fact]
    public void AddDandyMigrations_WithPostgres_RegistersPostgresServices()
    {
        const string connectionString = "Host=localhost;Database=migrations";
        var services = new ServiceCollection();

        services.AddMigrations(configuration =>
        {
            configuration.UsePostgres(connectionString);
            configuration.AddMigration<Migration1>();
        });

        using var serviceProvider = services.BuildServiceProvider();
        var connectionFactory = serviceProvider.GetRequiredService<IDbConnectionFactory>();
        var connection = Assert.IsType<NpgsqlConnection>(connectionFactory.Create());

        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.IsType<PostgresMigrationsSqlStrings>(serviceProvider.GetRequiredService<MigrationsSqlStrings>());
        Assert.IsType<IMigrationRunner>(serviceProvider.GetRequiredService<IMigrationRunner>(), exactMatch: false);

        var migration1 = Assert.Single(serviceProvider.GetServices<IMigration>());
        Assert.IsType<Migration1>(migration1);
    }

    [Fact]
    public void AddDandyMigrations_WithPostgresAndServiceKey_RegistersKeyedPostgresServices()
    {
        const string connectionString = "Host=localhost;Database=keyed-migrations";
        const string serviceKey = "postgres-migrations";
        var services = new ServiceCollection();

        services.AddMigrations(configuration =>
        {
            configuration.ServiceKey = serviceKey;
            configuration.UsePostgres(connectionString);
        });

        using var serviceProvider = services.BuildServiceProvider();
        var connectionFactory = serviceProvider.GetRequiredKeyedService<IDbConnectionFactory>(serviceKey);
        var connection = Assert.IsType<NpgsqlConnection>(connectionFactory.Create());

        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.Null(serviceProvider.GetService<IDbConnectionFactory>());
        Assert.IsType<PostgresMigrationsSqlStrings>(serviceProvider.GetRequiredKeyedService<MigrationsSqlStrings>(serviceKey));
    }
}
