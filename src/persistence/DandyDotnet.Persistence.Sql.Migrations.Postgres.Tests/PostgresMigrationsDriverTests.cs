using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests;

public sealed class PostgresMigrationsDriverTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void ConfigureServices_ShouldRegisterConnectionFactoryThatConnectsToPostgres()
    {
        var factory = fixture.GetService<IDbConnectionFactory>();

        Assert.NotNull(factory);

        using var connection = factory.Create();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT current_database();";

        var database = command.ExecuteScalar();

        Assert.Equal("dandy_dotnet_tests", database);
    }

    [Fact]
    public void ConfigureServices_ShouldRegisterPostgresMigrationSqlStrings()
    {
        var sqlStrings = fixture.GetService<MigrationsSqlStrings>();

        Assert.NotNull(sqlStrings);
        Assert.Contains("\"public\".\"__migrations\"", sqlStrings.CreateMigrationsTable);
        Assert.Contains("information_schema.schemata", sqlStrings.SchemaExists);
        Assert.Contains("information_schema.tables", sqlStrings.MigrationsTableExists);
    }

    [Fact]
    public void ConfigureServices_WithServiceKey_ShouldRegisterKeyedServices()
    {
        const string serviceKey = "orders";
        var services = new ServiceCollection();

        services.AddDandyMigrations(configuration =>
        {
            configuration.ServiceKey = serviceKey;
            configuration.UsePostgres(fixture.ConnectionString);
        });

        using var provider = services.BuildServiceProvider();

        var keyedFactory = provider.GetRequiredKeyedService<IDbConnectionFactory>(serviceKey);
        var defaultFactory = provider.GetService<IDbConnectionFactory>();

        Assert.NotNull(keyedFactory);
        Assert.Null(defaultFactory);
    }
}
