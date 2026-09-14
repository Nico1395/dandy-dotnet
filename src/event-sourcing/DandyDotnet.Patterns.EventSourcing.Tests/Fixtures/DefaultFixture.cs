using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private SqliteConnection? _keepAliveConnection;

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        if (_keepAliveConnection is not null)
            await _keepAliveConnection.DisposeAsync();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        try
        {
            _keepAliveConnection = new SqliteConnection("Data Source=Tests;Mode=Memory;Cache=Shared");
            _keepAliveConnection.Open();

            services.AddDandySerializer(cfg => cfg.UseSystemTextJson());
            services.AddDandyEventSourcing(cfg =>
            {
                cfg.ScanInAssemblies(typeof(DefaultFixture).Assembly);
                cfg.UseSqlite(sqlite =>
                {
                    sqlite.WithConnectionString("Data Source=Tests;Mode=Memory;Cache=Shared");
                });
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public IEventStore GetEventStore()
    {
        return ServiceProvider.GetRequiredService<IEventStore>();
    }

    protected override async Task OnInitializeAsync()
    {
        await base.OnInitializeAsync();
        
        var migrationRunner = ServiceProvider.GetRequiredKeyedService<IMigrationRunner>(EventSourcingConstants.ServiceKey);
        migrationRunner.MigrateUp();
    }
}