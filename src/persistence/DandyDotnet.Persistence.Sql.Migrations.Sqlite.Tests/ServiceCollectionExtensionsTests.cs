using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests;

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
    public void AddDandyMigrations_WithSqlite_RegistersSqliteServices()
    {
        const string connectionString = "Data Source=migrations";
        var services = new ServiceCollection();

        services.AddMigrations(configuration =>
        {
            configuration.UseSqlite(connectionString);
            configuration.AddMigration<Migration1>();
        });

        using var serviceProvider = services.BuildServiceProvider();
        var connectionFactory = serviceProvider.GetRequiredService<IDbConnectionFactory>();
        var connection = Assert.IsType<SqliteConnection>(connectionFactory.Create());

        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.IsType<SqliteMigrationsSqlStrings>(serviceProvider.GetRequiredService<MigrationsSqlStrings>());
        Assert.IsType<IMigrationRunner>(serviceProvider.GetRequiredService<IMigrationRunner>(), exactMatch: false);

        var migration1 = Assert.Single(serviceProvider.GetServices<IMigration>());
        Assert.IsType<Migration1>(migration1);
    }

    [Fact]
    public void AddDandyMigrations_WithSqliteAndServiceKey_RegistersKeyedSqliteServices()
    {
        const string connectionString = "Data Source=keyed-migrations";
        const string serviceKey = "sqlite-migrations";
        var services = new ServiceCollection();

        services.AddMigrations(configuration =>
        {
            configuration.ServiceKey = serviceKey;
            configuration.UseSqlite(connectionString);
        });

        using var serviceProvider = services.BuildServiceProvider();
        var connectionFactory = serviceProvider.GetRequiredKeyedService<IDbConnectionFactory>(serviceKey);
        var connection = Assert.IsType<SqliteConnection>(connectionFactory.Create());

        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.Null(serviceProvider.GetService<IDbConnectionFactory>());
        Assert.IsType<SqliteMigrationsSqlStrings>(serviceProvider.GetRequiredKeyedService<MigrationsSqlStrings>(serviceKey));
    }
}
