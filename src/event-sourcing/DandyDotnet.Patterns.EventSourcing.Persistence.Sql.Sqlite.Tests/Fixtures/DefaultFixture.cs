using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private const string ConnectionString = "Data Source=event-sourcing-tests;Mode=Memory;Cache=Shared";
    private SqliteConnection _keepAliveConnection = null!;

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSerialization(cfg => cfg.UseSystemTextJson());
        services.AddEventSourcing(cfg =>
        {
            cfg.ScanInAssemblies(typeof(DefaultFixture).Assembly);
            cfg.Outbox.DisableDaemon();
            cfg.Events.AddEvent<Mocks.ExpiringEvent>(eventConfiguration => eventConfiguration.WithLifetime(TimeSpan.Zero));
            cfg.UseSqlite(sqlite => sqlite.WithConnectionString(ConnectionString));
        });

        _keepAliveConnection = new SqliteConnection(ConnectionString);
        _keepAliveConnection.Open();
    }

    public IServiceScope CreateScope() => ServiceProvider.CreateScope();

    public SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public void ResetDatabase()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM outbox_envelope_consumers; DELETE FROM outbox_envelopes; DELETE FROM snapshots; DELETE FROM envelopes;";
        command.ExecuteNonQuery();
    }

    protected override Task OnInitializeAsync()
    {
        var migrationRunner = ServiceProvider.GetRequiredKeyedOrDefaultService<IMigrationRunner>(EventSourcingConstants.ServiceKey);
        migrationRunner.MigrateUp();
        
        return Task.CompletedTask;
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        _keepAliveConnection.Dispose();
    }
}
