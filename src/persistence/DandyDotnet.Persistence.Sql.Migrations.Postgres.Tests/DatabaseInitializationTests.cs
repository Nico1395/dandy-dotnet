using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Fixtures;
using DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests;

public sealed class DatabaseInitializationTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void InitializeDatabase_CreatesConfiguredSchemaAndMigrationsTable()
    {
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        Assert.Equal(0, CountSchema(schema));
        Assert.Equal(0, CountTable(schema, table));

        runner.InitializeDatabase();

        Assert.Equal(1, CountSchema(schema));
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

        Assert.Equal(1, CountSchema(schema));
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
            configuration.UsePostgres(fixture.ConnectionString);

            if (includeMigration)
                configuration.AddMigration<Migration1>();
        });

        return services.BuildServiceProvider();
    }

    private int CountSchema(string schema)
    {
        return ExecuteScalar<int>(
            "SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = @schema;",
            ("schema", schema));
    }

    private int CountTable(string schema, string table)
    {
        return ExecuteScalar<int>(
            """
            SELECT COUNT(*)
            FROM information_schema.tables
            WHERE table_schema = @schema AND table_name = @table;
            """,
            ("schema", schema),
            ("table", table));
    }

    private T ExecuteScalar<T>(string sql, params (string Name, object Value)[] parameters)
    {
        using var connection = new NpgsqlConnection(fixture.ConnectionString);
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
