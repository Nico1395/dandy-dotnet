using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests.Mocks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests;

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
    public void AddDandyMigrations_WithSqlServer_RegistersSqlServerServices()
    {
        const string connectionString = "Server=localhost;Database=migrations";
        var services = new ServiceCollection();

        services.AddMigrations(configuration =>
        {
            configuration.UseSqlServer(connectionString);
            configuration.AddMigration<Migration1>();
        });

        using var serviceProvider = services.BuildServiceProvider();
        var connectionFactory = serviceProvider.GetRequiredService<IDbConnectionFactory>();
        var connection = Assert.IsType<SqlConnection>(connectionFactory.Create());

        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.IsType<SqlServerMigrationsSqlStrings>(serviceProvider.GetRequiredService<MigrationsSqlStrings>());
        Assert.IsType<IMigrationRunner>(serviceProvider.GetRequiredService<IMigrationRunner>(), exactMatch: false);

        var migration1 = Assert.Single(serviceProvider.GetServices<IMigration>());
        Assert.IsType<Migration1>(migration1);
    }

    [Fact]
    public void AddDandyMigrations_WithSqlServerAndServiceKey_RegistersKeyedSqlServerServices()
    {
        const string connectionString = "Server=localhost;Database=keyed-migrations";
        const string serviceKey = "sql-server-migrations";
        var services = new ServiceCollection();

        services.AddMigrations(configuration =>
        {
            configuration.ServiceKey = serviceKey;
            configuration.UseSqlServer(connectionString);
        });

        using var serviceProvider = services.BuildServiceProvider();
        var connectionFactory = serviceProvider.GetRequiredKeyedService<IDbConnectionFactory>(serviceKey);
        var connection = Assert.IsType<SqlConnection>(connectionFactory.Create());

        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.Null(serviceProvider.GetService<IDbConnectionFactory>());
        Assert.IsType<SqlServerMigrationsSqlStrings>(serviceProvider.GetRequiredKeyedService<MigrationsSqlStrings>(serviceKey));
    }
}
