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

    public long[] GetAppliedVersions()
    {
        InitializeDatabase();

        using var connection = GetOpenDbConnection();
        return GetAppliedVersions(connection);
    }

    public bool HasUnappliedMigrations()
    {
        var appliedVersions = GetAppliedVersions();
        var migrations = GetMigrations()
            .OrderBy(m => m.Version)
            .ToArray();

        return HasUnappliedMigrations(appliedVersions, migrations);
    }

    public void InitializeDatabase()
    {
        using var connection = GetOpenDbConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            if (!SchemaExists(connection, transaction))
                CreateSchema(connection, transaction);

            if (MigrationsTableExists(connection, transaction))
                return;

            var sqlStrings = GetSqlStrings();
            connection.Execute(new CommandDefinition(sqlStrings.CreateMigrationsTable, transaction: transaction));

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            configuration.OnExceptionDuringCreatingSchemaAndTable?.Invoke(serviceProvider, ex);
            Console.WriteLine(ex);

            throw;
        }
    }

    public void MigrateUp()
    {
        InitializeDatabase();

        var migrations = GetMigrations()
            .OrderBy(m => m.Version)
            .ToArray();

        if (migrations.Length == 0)
            return;

        using var connection = GetOpenDbConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var appliedVersions = GetAppliedVersions(connection, transaction);
            if (!HasUnappliedMigrations(appliedVersions, migrations))
                return;

            long? lastVersion = appliedVersions.Length != 0 ? appliedVersions.Last() : null;
            var upMigrations = migrations;
            if (lastVersion.HasValue)
            {
                upMigrations = migrations
                    .Where(m =>
                        m.Version > lastVersion.Value ||
                        !appliedVersions.Contains(m.Version))
                    .ToArray();
            }

            var builder = new MigrationBuilder(connection, transaction);
            foreach (var migration in upMigrations)
                migration.Up(builder);

            InsertMigrations(upMigrations, connection, transaction);
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
        InitializeDatabase();

        var migrations = GetMigrations()
            .OrderByDescending(m => m.Version)
            .Where(m => toVersion == null || m.Version >= toVersion)
            .ToArray();

        if (migrations.Length == 0)
            return;

        using var connection = GetOpenDbConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var appliedVersions = GetAppliedVersions(connection, transaction);
            var downMigrations = migrations
                .Where(m => appliedVersions.Contains(m.Version))
                .OrderByDescending(m => m.Version)
                .ToArray();

            var builder = new MigrationBuilder(connection, transaction);
            foreach (var migration in downMigrations)
                migration.Down(builder);

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
            return connection;
        }
        catch (Exception exception)
        {
            configuration.OnExceptionDuringOpeningConnection?.Invoke(serviceProvider, exception);
            Console.WriteLine(exception);
            throw;
        }
    }

    private IDbConnectionFactory GetDbConnectionFactory()
    {
        return _dbConnectionFactory ??= configuration.ServiceKey == null
            ? serviceProvider.GetRequiredService<IDbConnectionFactory>()
            : serviceProvider.GetRequiredKeyedService<IDbConnectionFactory>(configuration.ServiceKey);
    }

    private IEnumerable<IMigration> GetMigrations()
    {
        return configuration.ServiceKey == null
            ? serviceProvider.GetServices<IMigration>()
            : serviceProvider.GetKeyedServices<IMigration>(configuration.ServiceKey);
    }

    private MigrationsSqlStrings GetSqlStrings()
    {
        return _sqlStrings ??= configuration.ServiceKey == null
            ? serviceProvider.GetRequiredService<MigrationsSqlStrings>()
            : serviceProvider.GetRequiredKeyedService<MigrationsSqlStrings>(configuration.ServiceKey);
    }

    private long[] GetAppliedVersions(IDbConnection connection, IDbTransaction? transaction = null)
    {
        var sqlStrings = GetSqlStrings();
        var versions = connection.Query<long>(new CommandDefinition(sqlStrings.GetAppliedVersions, transaction: transaction));
        return versions.Order().ToArray();
    }

    private bool HasUnappliedMigrations(long[] appliedVersions, IMigration[] migrations)
    {
        return migrations.Any(m => !appliedVersions.Contains(m.Version));
    }

    private bool SchemaExists(IDbConnection connection, IDbTransaction? transaction = null)
    {
        var sqlStrings = GetSqlStrings();
        var result = connection.ExecuteScalar<int>(new CommandDefinition(sqlStrings.SchemaExists, transaction: transaction));

        return result > 0;
    }

    private void CreateSchema(IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        connection.Execute(new CommandDefinition(sqlStrings.CreateSchema, transaction: transaction));
    }

    private bool MigrationsTableExists(IDbConnection connection, IDbTransaction? transaction = null)
    {
        var sqlStrings = GetSqlStrings();
        var result = connection.ExecuteScalar<int>(new CommandDefinition(sqlStrings.MigrationsTableExists, transaction: transaction));

        return result > 0;
    }

    private void InsertMigrations(IMigration[] migrations, IDbConnection connection, IDbTransaction transaction)
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