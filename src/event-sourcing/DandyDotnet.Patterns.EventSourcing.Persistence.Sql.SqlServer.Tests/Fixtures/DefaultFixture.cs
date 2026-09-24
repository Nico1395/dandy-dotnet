using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private MsSqlContainer? _sqlServer;

    public string ConnectionString => _sqlServer?.GetConnectionString() ?? throw new InvalidOperationException("The SQL Server container has not been started.");

    protected override void ConfigureServices(IServiceCollection services)
    {
        _sqlServer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .WithDatabase("tests")
            .WithPassword("Testcontainers1!")
            .Build();
        _sqlServer.StartAsync().GetAwaiter().GetResult();

        services.AddSerialization(cfg => cfg.UseSystemTextJson());
        services.AddEventSourcing(cfg =>
        {
            cfg.ScanInAssemblies(typeof(DefaultFixture).Assembly);
            cfg.Outbox.DisableDaemon();
            cfg.Events.AddEvent<Mocks.ExpiringEvent>(eventConfiguration => eventConfiguration.WithLifetime(TimeSpan.Zero));
            cfg.UseSqlServer(sqlServer => sqlServer.WithConnectionString(ConnectionString));
        });
    }

    public IServiceScope CreateScope() => ServiceProvider.CreateScope();

    public SqlConnection OpenConnection()
    {
        var connection = new SqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public void ResetDatabase()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM [event_store].[outbox_envelope_consumers]; DELETE FROM [event_store].[outbox_envelopes]; DELETE FROM [event_store].[snapshots]; DELETE FROM [event_store].[envelopes];";
        command.ExecuteNonQuery();
    }

    protected override Task OnInitializeAsync()
    {
        ServiceProvider.GetRequiredKeyedService<IMigrationRunner>(EventSourcingConstants.ServiceKey).MigrateUp();
        return Task.CompletedTask;
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        if (_sqlServer is not null)
            await _sqlServer.DisposeAsync();
    }
}
