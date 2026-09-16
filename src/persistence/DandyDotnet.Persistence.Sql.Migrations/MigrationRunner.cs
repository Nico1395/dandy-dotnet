using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Implementation of <see cref="IMigrationRunner" /> that manages and executes database migrations.
/// </summary>
/// <remarks>
///     <para>
///         This class provides the core functionality for database migration management, including
///         initializing the database schema, tracking applied migrations, applying pending migrations,
///         and rolling back migrations.
///     </para>
///     <para>
///         The MigrationRunner uses dependency injection to resolve database connections, SQL string
///         providers, and migrations based on the configured service key.
///     </para>
/// </remarks>
internal sealed class MigrationRunner(
    MigrationsConfiguration configuration,
    IServiceProvider serviceProvider) : IMigrationRunner
{
    private IDbConnectionFactory? _dbConnectionFactory;
    private MigrationsSqlStrings? _sqlStrings;

    /// <summary>
    ///     Gets an array of version numbers for all migrations that have been applied to the database.
    /// </summary>
    /// <returns>An array of long integers representing the versions of applied migrations, ordered by version.</returns>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during database connection or query execution.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method ensures the database schema and migrations table exist by calling
    ///         <see cref="InitializeDatabase" /> before querying for applied migrations.
    ///     </para>
    ///     <para>
    ///         The returned array is ordered in ascending order by version number.
    ///     </para>
    /// </remarks>
    public long[] GetAppliedVersions()
    {
        InitializeDatabase();

        using var connection = GetOpenDbConnection();
        return GetAppliedVersions(connection);
    }

    /// <summary>
    ///     Determines whether there are any migrations that have not been applied to the database.
    /// </summary>
    /// <returns><see langword="true" /> if there are unapplied migrations; otherwise, <see langword="false" />.</returns>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during database connection or query execution.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method compares the list of registered migrations with the list of applied migrations
    ///         to determine if any migrations remain to be applied.
    ///     </para>
    ///     <para>
    ///         The method retrieves all registered migrations through dependency injection and checks
    ///         if any of them have not been recorded in the database as applied.
    ///     </para>
    /// </remarks>
    public bool HasUnappliedMigrations()
    {
        var appliedVersions = GetAppliedVersions();
        var migrations = GetMigrations()
            .OrderBy(m => m.Version)
            .ToArray();

        return HasUnappliedMigrations(appliedVersions, migrations);
    }

    /// <summary>
    ///     Initializes the database schema and migrations table.
    /// </summary>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during database schema or table creation. The exception is
    ///     passed to the <see cref="MigrationsConfiguration.OnExceptionDuringCreatingSchemaAndTable" />
    ///     callback if configured.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method ensures that the required database schema and migrations table exist.
    ///         If the schema does not exist, it will be created. If the migrations table does not exist,
    ///         it will be created with the appropriate structure.
    ///     </para>
    ///     <para>
    ///         All operations are executed within a transaction. If any error occurs, the transaction
    ///         is rolled back and the exception is rethrown after invoking the configured exception handler.
    ///     </para>
    ///     <para>
    ///         If the schema and migrations table already exist, this method simply returns without
    ///         performing any action.
    ///     </para>
    /// </remarks>
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

    /// <summary>
    ///     Applies all pending migrations to the database in ascending order of their version numbers.
    /// </summary>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during migration execution. The exception is passed to the
    ///     <see cref="MigrationsConfiguration.OnExceptionDuringMigrateUp" /> callback if configured.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method applies all migrations that have not yet been applied to the database.
    ///         Migrations are executed in ascending order of their version numbers.
    ///     </para>
    ///     <para>
    ///         The method first ensures the database schema and migrations table exist by calling
    ///         <see cref="InitializeDatabase" />, then identifies which migrations need to be applied.
    ///     </para>
    ///     <para>
    ///         All migration operations are executed within a transaction. If any migration fails, the
    ///         entire transaction is rolled back and the exception is rethrown after invoking the
    ///         configured exception handler.
    ///     </para>
    ///     <para>
    ///         After each successful migration, a record is inserted into the migrations table to track
    ///         that the migration has been applied.
    ///     </para>
    /// </remarks>
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

    /// <summary>
    ///     Rolls back migrations from the database to the specified version.
    /// </summary>
    /// <param name="toVersion">
    ///     The target version to roll back to, or <see langword="null" /> to roll back all applied migrations.
    /// </param>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during rollback execution. The exception is passed to the
    ///     <see cref="MigrationsConfiguration.OnExceptionDuringMigrateDown" /> callback if configured.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method rolls back migrations in descending order of their version numbers.
    ///         If <paramref name="toVersion" /> is <see langword="null" />, all applied migrations will be rolled back.
    ///         If <paramref name="toVersion" /> is specified, only migrations with version greater than or equal to
    ///         the specified version will be considered for rollback.
    ///     </para>
    ///     <para>
    ///         The method first ensures the database schema and migrations table exist by calling
    ///         <see cref="InitializeDatabase" />, then identifies which migrations need to be rolled back.
    ///     </para>
    ///     <para>
    ///         All rollback operations are executed within a transaction. If any rollback fails, the
    ///         entire transaction is rolled back and the exception is rethrown after invoking the
    ///         configured exception handler.
    ///     </para>
    ///     <para>
    ///         Only migrations that are recorded in the database as applied will have their <see cref="IMigration.Down" />
    ///         method called. After each successful rollback, the corresponding record is removed from the
    ///         migrations table.
    ///     </para>
    /// </remarks>
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

    /// <summary>
    ///     Gets an open database connection for migration operations.
    /// </summary>
    /// <returns>An open <see cref="IDbConnection" /> instance.</returns>
    /// <exception cref="Exception">
    ///     Thrown when an error occurs during database connection. The exception is passed to the
    ///     <see cref="MigrationsConfiguration.OnExceptionDuringOpeningConnection" /> callback if configured.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method creates a new database connection using the configured connection factory and
    ///         attempts to open it. If the connection fails to open, the exception is passed to the configured
    ///         exception handler and then rethrown.
    ///     </para>
    ///     <para>
    ///         The caller is responsible for disposing the returned connection when finished.
    ///     </para>
    /// </remarks>
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

    /// <summary>
    ///     Gets the database connection factory based on the configuration.
    /// </summary>
    /// <returns>An <see cref="IDbConnectionFactory" /> instance.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection factory cannot be resolved from the service provider.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method resolves the <see cref="IDbConnectionFactory" /> from the service provider based on
    ///         the configured service key. If <see cref="MigrationsConfiguration.ServiceKey" /> is <see langword="null" />,
    ///         it uses the non-keyed resolution; otherwise, it uses keyed resolution.
    ///     </para>
    ///     <para>
    ///         The connection factory is cached for subsequent calls to improve performance.
    ///     </para>
    /// </remarks>
    private IDbConnectionFactory GetDbConnectionFactory()
    {
        return _dbConnectionFactory ??= configuration.ServiceKey == null
            ? serviceProvider.GetRequiredService<IDbConnectionFactory>()
            : serviceProvider.GetRequiredKeyedService<IDbConnectionFactory>(configuration.ServiceKey);
    }

    /// <summary>
    ///     Gets all registered migrations from the service provider.
    /// </summary>
    /// <returns>An enumerable of <see cref="IMigration" /> instances.</returns>
    /// <remarks>
    ///     <para>
    ///         This method resolves all registered <see cref="IMigration" /> instances from the service provider
    ///         based on the configured service key. If <see cref="MigrationsConfiguration.ServiceKey" /> is
    ///         <see langword="null" />, it uses the non-keyed resolution; otherwise, it uses keyed resolution.
    ///     </para>
    /// </remarks>
    private IEnumerable<IMigration> GetMigrations()
    {
        return configuration.ServiceKey == null
            ? serviceProvider.GetServices<IMigration>()
            : serviceProvider.GetKeyedServices<IMigration>(configuration.ServiceKey);
    }

    /// <summary>
    ///     Gets the SQL strings provider based on the configuration.
    /// </summary>
    /// <returns>An <see cref="MigrationsSqlStrings" /> instance.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the SQL strings provider cannot be resolved from the service provider.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method resolves the <see cref="MigrationsSqlStrings" /> from the service provider based on
    ///         the configured service key. If <see cref="MigrationsConfiguration.ServiceKey" /> is <see langword="null" />,
    ///         it uses the non-keyed resolution; otherwise, it uses keyed resolution.
    ///     </para>
    ///     <para>
    ///         The SQL strings provider is cached for subsequent calls to improve performance.
    ///     </para>
    /// </remarks>
    private MigrationsSqlStrings GetSqlStrings()
    {
        return _sqlStrings ??= configuration.ServiceKey == null
            ? serviceProvider.GetRequiredService<MigrationsSqlStrings>()
            : serviceProvider.GetRequiredKeyedService<MigrationsSqlStrings>(configuration.ServiceKey);
    }

    /// <summary>
    ///     Gets an array of version numbers for all migrations that have been applied to the database.
    /// </summary>
    /// <param name="connection">The database connection to use for the query.</param>
    /// <param name="transaction">Optional transaction to use for the query.</param>
    /// <returns>An array of long integers representing the versions of applied migrations, ordered by version.</returns>
    /// <remarks>
    ///         This method queries the migrations table using the SQL string provided by <see cref="GetSqlStrings" />.
    ///         The returned array is ordered in ascending order by version number.
    ///     </remarks>
    private long[] GetAppliedVersions(IDbConnection connection, IDbTransaction? transaction = null)
    {
        var sqlStrings = GetSqlStrings();
        var versions = connection.Query<long>(new CommandDefinition(sqlStrings.GetAppliedVersions, transaction: transaction));
        return versions.Order().ToArray();
    }

    /// <summary>
    ///     Determines whether there are any migrations in the provided list that have not been applied.
    /// </summary>
    /// <param name="appliedVersions">Array of version numbers for applied migrations.</param>
    /// <param name="migrations">Array of migrations to check.</param>
    /// <returns><see langword="true" /> if there are unapplied migrations; otherwise, <see langword="false" />.</returns>
    private bool HasUnappliedMigrations(long[] appliedVersions, IMigration[] migrations)
    {
        return migrations.Any(m => !appliedVersions.Contains(m.Version));
    }

    /// <summary>
    ///     Checks if the database schema exists.
    /// </summary>
    /// <param name="connection">The database connection to use for the check.</param>
    /// <param name="transaction">Optional transaction to use for the check.</param>
    /// <returns><see langword="true" /> if the schema exists; otherwise, <see langword="false" />.</returns>
    /// <remarks>
    ///         This method uses the SQL string provided by <see cref="MigrationsSqlStrings.SchemaExists" /> to
    ///         determine if the schema exists in the database.
    ///     </remarks>
    private bool SchemaExists(IDbConnection connection, IDbTransaction? transaction = null)
    {
        var sqlStrings = GetSqlStrings();
        var result = connection.ExecuteScalar<int>(new CommandDefinition(sqlStrings.SchemaExists, transaction: transaction));

        return result > 0;
    }

    /// <summary>
    ///     Creates the database schema if it does not exist.
    /// </summary>
    /// <param name="connection">The database connection to use for creating the schema.</param>
    /// <param name="transaction">The transaction to use for creating the schema.</param>
    /// <remarks>
    ///         This method uses the SQL string provided by <see cref="MigrationsSqlStrings.CreateSchema" /> to
    ///         create the schema in the database.
    ///     </remarks>
    private void CreateSchema(IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        connection.Execute(new CommandDefinition(sqlStrings.CreateSchema, transaction: transaction));
    }

    /// <summary>
    ///     Checks if the migrations table exists in the database.
    /// </summary>
    /// <param name="connection">The database connection to use for the check.</param>
    /// <param name="transaction">Optional transaction to use for the check.</param>
    /// <returns><see langword="true" /> if the migrations table exists; otherwise, <see langword="false" />.</returns>
    /// <remarks>
    ///         This method uses the SQL string provided by <see cref="MigrationsSqlStrings.MigrationsTableExists" /> to
    ///         determine if the migrations table exists in the database.
    ///     </remarks>
    private bool MigrationsTableExists(IDbConnection connection, IDbTransaction? transaction = null)
    {
        var sqlStrings = GetSqlStrings();
        var result = connection.ExecuteScalar<int>(new CommandDefinition(sqlStrings.MigrationsTableExists, transaction: transaction));

        return result > 0;
    }

    /// <summary>
    ///     Inserts records for the specified migrations into the migrations table.
    /// </summary>
    /// <param name="migrations">The migrations to insert records for.</param>
    /// <param name="connection">The database connection to use for the insert operation.</param>
    /// <param name="transaction">The transaction to use for the insert operation.</param>
    /// <remarks>
    ///         This method inserts a record for each migration in the <paramref name="migrations" /> array into
    ///         the migrations table using the SQL string provided by <see cref="MigrationsSqlStrings.InsertMigrations" />.
    ///         Each record includes the migration version number and the current timestamp.
    ///     </remarks>
    private void InsertMigrations(IMigration[] migrations, IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        var parameters = migrations.Select(m => new { m.Version }).ToArray();

        connection.Execute(new CommandDefinition(sqlStrings.InsertMigrations, parameters, transaction: transaction));
    }

    /// <summary>
    ///     Deletes records for the specified migrations from the migrations table.
    /// </summary>
    /// <param name="migrations">The migrations to delete records for.</param>
    /// <param name="connection">The database connection to use for the delete operation.</param>
    /// <param name="transaction">The transaction to use for the delete operation.</param>
    /// <remarks>
    ///         This method deletes the record for each migration in the <paramref name="migrations" /> array from
    ///         the migrations table using the SQL string provided by <see cref="MigrationsSqlStrings.DeleteMigrations" />.
    ///         Each migration record is identified by its version number.
    ///     </remarks>
    private void DeleteMigrations(IMigration[] migrations, IDbConnection connection, IDbTransaction transaction)
    {
        var sqlStrings = GetSqlStrings();
        var parameters = migrations.Select(m => new { m.Version }).ToArray();

        connection.Execute(new CommandDefinition(sqlStrings.DeleteMigrations, parameters, transaction: transaction));
    }
}