using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

internal sealed class MigrationRunner(
    MigrationsConfiguration configuration,
    IServiceProvider serviceProvider) : IMigrationRunner
{
    private IDbConnectionFactory? _dbConnectionFactory;
    private MigrationsSqlStrings? _sqlStrings;

    public void MigrateUp()
    {
        var migrations = serviceProvider
            .GetServices<IMigration>()
            .OrderBy(m => m.Version)
            .ToArray();

        if (migrations.Length == 0)
            return;

        var connection = GetOpenDbConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var builder = new MigrationBuilder(connection, transaction);
            foreach (var migration in migrations)
                migration.Up(builder);

            EnsureMigrationsTableIsCreated(connection, transaction);
            LogMigrations(migrations, connection, transaction);

            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            configuration.OnExceptionDuringMigrateUp?.Invoke(serviceProvider, exception);
            Console.WriteLine(exception);

            throw;
        }
    }

    public void MigrateDown(long? toVersion)
    {
        var connection = GetOpenDbConnection();
        if (!MigrationsTableExists(connection))
            return;

        var migrations = serviceProvider
            .GetServices<IMigration>()
            .OrderByDescending(m => m.Version)
            .Where(m => toVersion == null || m.Version >= toVersion)
            .ToArray();

        if (migrations.Length == 0)
            return;

        using var transaction = connection.BeginTransaction();

        try
        {
            var builder = new MigrationBuilder(connection, transaction);
            foreach (var migration in migrations)
                migration.Down(builder);

            EnsureMigrationsTableIsCreated(connection, transaction);
            DeleteMigrations(migrations, connection, transaction);

            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            configuration.OnExceptionDuringMigrateDown?.Invoke(serviceProvider, exception);
            Console.WriteLine(exception);

            throw;
        }
    }

    private IDbConnection GetOpenDbConnection()
    {
        var connection = GetDbConnectionFactory().Create();

        try
        {
            connection.Open();
        }
        catch (Exception exception)
        {
            configuration.OnExceptionDuringOpeningConnection?.Invoke(serviceProvider, exception);
            Console.WriteLine(exception);
        }

        return connection;
    }
    
    private IDbConnectionFactory GetDbConnectionFactory()
    {
        return _dbConnectionFactory ??= configuration.ServiceKey == null
            ? serviceProvider.GetRequiredService<IDbConnectionFactory>()
            : serviceProvider.GetRequiredKeyedService<IDbConnectionFactory>(configuration.ServiceKey);
    }

    private MigrationsSqlStrings GetSqlStrings()
    {
        return _sqlStrings ??= configuration.ServiceKey == null
            ? serviceProvider.GetRequiredService<MigrationsSqlStrings>()
            : serviceProvider.GetRequiredKeyedService<MigrationsSqlStrings>(configuration.ServiceKey);
    }

    private void EnsureMigrationsTableIsCreated(IDbConnection connection, IDbTransaction transaction)
    {
        if (!SchemaExists(connection))
            CreateSchema(connection, transaction);

        if (MigrationsTableExists(connection))
            return;

        var sqlStrings = GetSqlStrings();
        connection.Execute(new CommandDefinition(sqlStrings.CreateMigrationsTable, transaction: transaction));
    }

    private bool SchemaExists(IDbConnection connection)
    {
        var sqlStrings = GetSqlStrings();
        var result = connection.ExecuteScalar<int>(new CommandDefinition(sqlStrings.SchemaExists));

        return result > 0;
    }

    private void CreateSchema(IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        connection.Execute(new CommandDefinition(sqlStrings.CreateSchema, transaction: transaction));
    }

    private bool MigrationsTableExists(IDbConnection connection)
    {
        var sqlStrings = GetSqlStrings();
        var result = connection.ExecuteScalar<int>(new CommandDefinition(sqlStrings.MigrationsTableExists));

        return result > 0;
    }

    private void LogMigrations(IMigration[] migrations, IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        var parameters = migrations.Select(m => new { m.Version }).ToArray();

        connection.Execute(new CommandDefinition(sqlStrings.InsertMigrations, parameters, transaction: transaction));
    }

    private void DeleteMigrations(IMigration[] migrations, IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        var parameters = migrations.Select(m => new { m.Version }).ToArray();

        connection.Execute(new CommandDefinition(sqlStrings.DeleteMigrations, parameters, transaction: transaction));
    }
}