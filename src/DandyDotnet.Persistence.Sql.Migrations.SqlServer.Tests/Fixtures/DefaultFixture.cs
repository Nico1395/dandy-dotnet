using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private MsSqlContainer? _sqlServer;

    public string ConnectionString => _sqlServer?.GetConnectionString() ?? throw new InvalidOperationException("The SQL Server container has not been started.");

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();

        if (_sqlServer is not null)
            await _sqlServer.DisposeAsync();
    }

    public IMigrationRunner GetMigrationRunner()
    {
        return ServiceProvider.GetRequiredService<IMigrationRunner>();
    }

    public IDbConnectionFactory GetDbConnectionFactory()
    {
        return ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    }

    public IEnumerable<IMigration> GetMigrations()
    {
        return ServiceProvider.GetServices<IMigration>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        var assemblies = new[] { typeof(DefaultFixture).Assembly };

        _sqlServer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .WithDatabase("tests")
            .WithPassword("Testcontainers1!")
            .Build();

        _sqlServer.StartAsync().GetAwaiter().GetResult();

        services.AddDandyMigrations(configuration =>
        {
            configuration.UseSqlServer(ConnectionString);
            configuration.ScanInAssemblies(assemblies);
        });
    }
}