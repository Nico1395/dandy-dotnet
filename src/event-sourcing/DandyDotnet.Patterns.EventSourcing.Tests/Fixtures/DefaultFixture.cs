using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Fixtures;

public sealed class DefaultFixture : IServiceProvider, IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    public DefaultFixture()
    {
        try
        {
            var services = new ServiceCollection();

            services.AddDandySerializer(cfg => cfg.UseSystemTextJson());
            services.AddDandyEventSourcing(cfg =>
            {
                cfg.ScanInAssemblies(typeof(DefaultFixture).Assembly);
                cfg.UseSqlite(sqlite =>
                {
                    sqlite.WithConnectionString("Data Source=Tests;Mode=Memory;Cache=Shared");
                });
            });

            _serviceProvider = services.BuildServiceProvider();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }

    public IEventStore GetEventStore()
    {
        return _serviceProvider.GetRequiredService<IEventStore>();
    }

    public Task InitializeAsync()
    {
        try
        {
            var migrationRunner = _serviceProvider.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}