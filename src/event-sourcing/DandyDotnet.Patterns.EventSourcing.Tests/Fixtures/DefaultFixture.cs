using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using DandyDotnet.Tests.Core.Fixtures;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        try
        {
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

    public override Task InitializeAsync()
    {
        try
        {
            var migrationRunner = ServiceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return Task.CompletedTask;
    }
}