using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private PostgreSqlContainer? _postgres;

    public string ConnectionString => _postgres?.GetConnectionString() ?? throw new InvalidOperationException("The PostgreSQL container has not been started.");

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();

        if (_postgres is not null)
            await _postgres.DisposeAsync();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        var assemblies = new[] { typeof(DefaultFixture).Assembly };

        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("tests")
            .WithUsername("dev")
            .WithPassword("dev")
            .Build();

        _postgres.StartAsync().GetAwaiter().GetResult();

        services.AddDandyMigrations(configuration =>
        {
            configuration.UsePostgres(ConnectionString);
            configuration.ScanInAssemblies(assemblies);
        });
    }
}