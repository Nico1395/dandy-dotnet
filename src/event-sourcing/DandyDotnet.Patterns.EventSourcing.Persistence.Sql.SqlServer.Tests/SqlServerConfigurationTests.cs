using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Fixtures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests;

public sealed class SqlServerConfigurationTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public SqlServerConfigurationTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }

    [Fact]
    public void UseSqlServer_ShouldRegisterSqlServerConnectionAndSqlStrings()
    {
        using var scope = fixture.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredKeyedService<DandyDotnet.Persistence.Sql.Abstractions.IDbConnectionFactory>(EventSourcingConstants.ServiceKey);
        using var connection = factory.Create();
        Assert.IsType<SqlConnection>(connection);
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<SqlStrings>());
    }

    [Fact]
    public void Migrations_ShouldCreateAllRequiredTables()
    {
        using var connection = fixture.OpenConnection();
        foreach (var table in new[] { Tables.Envelopes.Table, Tables.Snapshots.Table, Tables.OutboxEnvelopes.Table, Tables.OutboxEnvelopeConsumers.Table })
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table";
            command.Parameters.AddWithValue("@schema", Schema.Name);
            command.Parameters.AddWithValue("@table", table);
            Assert.Equal(1, command.ExecuteScalar());
        }
    }
}
