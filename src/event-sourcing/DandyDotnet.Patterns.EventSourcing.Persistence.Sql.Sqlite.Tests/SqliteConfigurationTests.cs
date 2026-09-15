using DandyDotnet.Patterns.EventSourcing.Persistence.Sql;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests;

public sealed class SqliteConfigurationTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public SqliteConfigurationTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }
    [Fact]
    public void UseSqlite_ShouldRegisterSqliteConnectionAndSqlStrings()
    {
        using var scope = fixture.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Connections.IDbConnectionFactory>();
        using var connection = factory.Create();
        Assert.IsType<SqliteConnection>(connection);
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<SqlStrings>());
    }

    [Fact]
    public void Migrations_ShouldCreateAllRequiredTables()
    {
        using var connection = fixture.OpenConnection();
        foreach (var table in new[] { Tables.Envelopes.Table, Tables.Snapshots.Table, Tables.OutboxEnvelopes.Table, Tables.OutboxEnvelopeConsumers.Table })
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = $name";
            command.Parameters.AddWithValue("$name", table);
            Assert.Equal(1L, command.ExecuteScalar());
        }
    }
}
