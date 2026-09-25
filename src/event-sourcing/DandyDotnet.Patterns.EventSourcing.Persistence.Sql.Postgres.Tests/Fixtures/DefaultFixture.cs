using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private PostgreSqlContainer? _postgres;

    public string ConnectionString => _postgres?.GetConnectionString() ?? throw new InvalidOperationException("The PostgreSQL container has not been started.");

    protected override void ConfigureServices(IServiceCollection services)
    {
        _postgres = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("tests")
            .WithUsername("dev")
            .WithPassword("dev")
            .Build();
        _postgres.StartAsync().GetAwaiter().GetResult();

        services.AddSerialization(cfg => cfg.UseSystemTextJson());
        services.AddEventSourcing(cfg =>
        {
            cfg.ScanInAssemblies(typeof(DefaultFixture).Assembly);
            cfg.Outbox.DisableDaemon();
            cfg.Events.AddEvent<Mocks.ExpiringEvent>(eventConfiguration => eventConfiguration.WithLifetime(TimeSpan.Zero));
            cfg.UseNpgsql(npgsql => npgsql.WithConnectionString(ConnectionString));
        });
    }

    public IServiceScope CreateScope() => ServiceProvider.CreateScope();

    public NpgsqlConnection OpenConnection()
    {
        var connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public void ResetDatabase()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "TRUNCATE TABLE event_store.outbox_envelope_consumers, event_store.outbox_envelopes, event_store.snapshots, event_store.envelopes CASCADE;";
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
        if (_postgres is not null)
            await _postgres.DisposeAsync();
    }
}
