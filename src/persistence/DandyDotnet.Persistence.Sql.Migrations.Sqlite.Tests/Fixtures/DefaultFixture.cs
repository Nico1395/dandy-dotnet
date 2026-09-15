using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private SqliteConnection? _keepAliveConnection;
    private string? _connectionString;

    public string ConnectionString =>
        _connectionString ?? throw new InvalidOperationException("The SQLite database has not been initialized.");

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        if (_keepAliveConnection is not null)
            await _keepAliveConnection.DisposeAsync();
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
        _connectionString = $"Data Source=migrations-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
        _keepAliveConnection = new SqliteConnection(ConnectionString);
        _keepAliveConnection.Open();

        services.AddMigrations(configuration =>
        {
            configuration.UseSqlite(ConnectionString);
            configuration.ScanInAssemblies(assemblies);
        });
    }
}
