using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests.Fixtures;
using DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests.Mocks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer.Tests;

public sealed class MigrateUpTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void MigrateUp_WithNoMigrations_InitializesDatabaseWithoutApplyingMigrations()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();

        Assert.Empty(MigrationExecutionRecorder.GetUpVersions());
        Assert.Empty(runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateUp_WithUnappliedMigrations_ExecutesThemInVersionOrder()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration2), typeof(RecordingMigration1));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();

        Assert.Equal([1, 2], MigrationExecutionRecorder.GetUpVersions());
        Assert.Equal([1, 2], runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateUp_WhenAllMigrationsAreApplied_DoesNotExecuteThemAgain()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration1), typeof(RecordingMigration2));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
        MigrationExecutionRecorder.Reset();

        runner.MigrateUp();

        Assert.Empty(MigrationExecutionRecorder.GetUpVersions());
        Assert.Equal([1, 2], runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateUp_WithPartiallyAppliedMigrations_ExecutesOnlyRemainingMigrations()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration1), typeof(RecordingMigration2));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.InitializeDatabase();
        InsertAppliedVersion(schema, table, 1);
        runner.MigrateUp();

        Assert.Equal([2], MigrationExecutionRecorder.GetUpVersions());
        Assert.Equal([1, 2], runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateUp_WhenMigrationFails_RollsBackChangesAndInvokesCallback()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        Exception? callbackException = null;
        using var serviceProvider = CreateServiceProvider(
            schema,
            table,
            configuration => configuration.OnExceptionDuringMigrateUp = (_, exception) => callbackException = exception,
            typeof(RecordingMigration1),
            typeof(FailingMigration));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        Assert.Throws<InvalidOperationException>(() => runner.MigrateUp());

        Assert.Equal([1, 3], MigrationExecutionRecorder.GetUpVersions());
        Assert.Empty(runner.GetAppliedVersions());
        Assert.IsType<InvalidOperationException>(callbackException);
    }

    private ServiceProvider CreateServiceProvider(
        string schema,
        string table,
        params Type[] migrationTypes)
    {
        return CreateServiceProvider(schema, table, _ => { }, migrationTypes);
    }

    private ServiceProvider CreateServiceProvider(
        string schema,
        string table,
        Action<MigrationsConfiguration> configure,
        params Type[] migrationTypes)
    {
        var services = new ServiceCollection();
        services.AddDandyMigrations(configuration =>
        {
            configuration.Schema = schema;
            configuration.Table = table;
            configuration.UseSqlServer(fixture.ConnectionString);
            configure(configuration);

            foreach (var migrationType in migrationTypes)
                configuration.MigrationTypes.Add(migrationType);
        });

        return services.BuildServiceProvider();
    }

    private void InsertAppliedVersion(string schema, string table, long version)
    {
        using var connection = new SqlConnection(fixture.ConnectionString);
        connection.Open();
        connection.Execute(
            $"INSERT INTO [{schema}].[{table}] ([version], [applied_at]) VALUES (@version, SYSUTCDATETIME());",
            new { version });
    }

    private static (string Schema, string Table) CreateUniqueIdentifiers()
    {
        var suffix = Guid.NewGuid().ToString("N");
        return ($"test_{suffix}", $"migrations_{suffix}");
    }
}
