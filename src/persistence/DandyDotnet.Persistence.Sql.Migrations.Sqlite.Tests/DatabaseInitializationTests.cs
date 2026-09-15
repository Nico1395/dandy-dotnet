using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Fixtures;
using DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests;

public sealed class DatabaseInitializationTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void InitializeDatabase_CreatesMigrationsTable()
    {
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        Assert.Equal(0, CountTable(schema, table));

        runner.InitializeDatabase();

        Assert.Equal(1, CountTable(schema, table));
    }

    [Fact]
    public void InitializeDatabase_IsIdempotent()
    {
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.InitializeDatabase();
        runner.InitializeDatabase();

        Assert.Equal(1, CountTable(schema, table));
    }

    [Fact]
    public void MigrationState_TracksAppliedMigrations()
    {
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, includeMigration: true);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.InitializeDatabase();

        Assert.Empty(runner.GetAppliedVersions());
        Assert.True(runner.HasUnappliedMigrations());

        runner.MigrateUp();

        Assert.Equal([1], runner.GetAppliedVersions());
        Assert.False(runner.HasUnappliedMigrations());
    }

    private ServiceProvider CreateServiceProvider(string schema, string table, bool includeMigration = false)
    {
        var services = new ServiceCollection();
        services.AddMigrations(configuration =>
        {
            configuration.Schema = schema;
            configuration.Table = table;
            configuration.UseSqlite(fixture.ConnectionString);

            if (includeMigration)
                configuration.AddMigration<Migration1>();
        });

        return services.BuildServiceProvider();
    }

    private int CountTable(string schema, string table)
    {
        return ExecuteScalar<int>(
            """
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = @table;
            """,
            ("table", table));
    }

    private T ExecuteScalar<T>(string sql, params (string Name, object Value)[] parameters)
    {
        using var connection = new SqliteConnection(fixture.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        foreach (var (name, value) in parameters)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        return (T)Convert.ChangeType(command.ExecuteScalar()!, typeof(T));
    }

    private static (string Schema, string Table) CreateUniqueIdentifiers()
    {
        var suffix = Guid.NewGuid().ToString("N");
        return ($"test_{suffix}", $"migrations_{suffix}");
    }
}
