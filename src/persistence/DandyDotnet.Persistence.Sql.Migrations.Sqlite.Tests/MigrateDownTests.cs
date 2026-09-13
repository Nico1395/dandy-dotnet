using Dapper;
using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Fixtures;
using DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite.Tests;

public sealed class MigrateDownTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void MigrateDown_WithNoMigrations_InitializesDatabaseWithoutApplyingMigrations()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateDown(null);

        Assert.Empty(MigrationExecutionRecorder.GetDownVersions());
        Assert.Empty(runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateDown_WithAllMigrationsApplied_ExecutesThemInReverseVersionOrder()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration1), typeof(RecordingMigration2));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        MigrationExecutionRecorder.Reset();

        runner.MigrateDown(null);

        Assert.Equal([2, 1], MigrationExecutionRecorder.GetDownVersions());
        Assert.Empty(runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateDown_WithTargetVersion_RollsBackTargetAndHigherVersions()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration1), typeof(RecordingMigration2));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        MigrationExecutionRecorder.Reset();

        runner.MigrateDown(2);

        Assert.Equal([2], MigrationExecutionRecorder.GetDownVersions());
        Assert.Equal([1], runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateDown_WhenNoMigrationsAreApplied_DoesNotExecuteThem()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration1), typeof(RecordingMigration2));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateDown(null);

        Assert.Empty(MigrationExecutionRecorder.GetDownVersions());
        Assert.Empty(runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateDown_WhenTargetIsAboveAllMigrations_DoesNotExecuteThem()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        using var serviceProvider = CreateServiceProvider(schema, table, typeof(RecordingMigration1), typeof(RecordingMigration2));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        MigrationExecutionRecorder.Reset();

        runner.MigrateDown(3);

        Assert.Empty(MigrationExecutionRecorder.GetDownVersions());
        Assert.Equal([1, 2], runner.GetAppliedVersions());
    }

    [Fact]
    public void MigrateDown_WhenMigrationFails_RollsBackChangesAndInvokesCallback()
    {
        MigrationExecutionRecorder.Reset();
        var (schema, table) = CreateUniqueIdentifiers();
        Exception? callbackException = null;
        using var serviceProvider = CreateServiceProvider(
            schema,
            table,
            configuration => configuration.OnExceptionDuringMigrateDown = (_, exception) => callbackException = exception,
            typeof(RecordingMigration1),
            typeof(FailingDownMigration));
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        MigrationExecutionRecorder.Reset();

        Assert.Throws<InvalidOperationException>(() => runner.MigrateDown(null));

        Assert.Equal([2], MigrationExecutionRecorder.GetDownVersions());
        Assert.Equal([1, 2], runner.GetAppliedVersions());
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
            configuration.UseSqlite(fixture.ConnectionString);
            configure(configuration);

            foreach (var migrationType in migrationTypes)
                configuration.MigrationTypes.Add(migrationType);
        });

        return services.BuildServiceProvider();
    }

    private static (string Schema, string Table) CreateUniqueIdentifiers()
    {
        var suffix = Guid.NewGuid().ToString("N");
        return ($"test_{suffix}", $"migrations_{suffix}");
    }
}
