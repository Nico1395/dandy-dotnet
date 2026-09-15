using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres.Tests;

public sealed class SqlPostgresConfigurationTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public SqlPostgresConfigurationTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }

    [Fact]
    public void UseNpgsql_ShouldRegisterNpgsqlConnectionAndSqlStrings()
    {
        using var scope = fixture.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredKeyedService<DandyDotnet.Persistence.Sql.Abstractions.IDbConnectionFactory>(EventSourcingConstants.ServiceKey);
        using var connection = factory.Create();
        Assert.IsType<NpgsqlConnection>(connection);
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<SqlStrings>());
    }

    [Fact]
    public void Migrations_ShouldCreateAllRequiredTables()
    {
        using var connection = fixture.OpenConnection();
        foreach (var table in new[] { Tables.Envelopes.Table, Tables.Snapshots.Table, Tables.OutboxEnvelopes.Table, Tables.OutboxEnvelopeConsumers.Table })
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @schema AND table_name = @table";
            command.Parameters.AddWithValue("schema", Schema.Name);
            command.Parameters.AddWithValue("table", table);
            Assert.Equal(1L, command.ExecuteScalar());
        }
    }
}
