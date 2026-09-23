using System.Reflection;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Configuration class for database migrations.
/// </summary>
/// <remarks>
///     <para>
///         This class is used to configure the database migrations framework with various settings
///         such as the service key, assemblies to scan for migrations, database schema, table name,
///         database driver, and exception handlers.
///     </para>
/// </remarks>
/// <example>
///     <code>
///         services.AddMigrations(config => config
///             .UsePostgres("Server=localhost;Database=mydb;User Id=postgres;Password=password;")
///             .ScanInAssemblies(typeof(MyMigration).Assembly)
///             .Schema("mydb_schema"));
///     </code>
/// </example>
public sealed class MigrationsConfiguration
{
    /// <summary>
    ///     Gets or sets the optional service key for keyed dependency injection.
    /// </summary>
    /// <value>The service key object, or <see langword="null" /> for non-keyed services.</value>
    /// <remarks>
    ///     <para>
    ///         When set, all migration-related services (connection factory, SQL strings, and migrations)
    ///         will be registered with this key in the service collection.
    ///     </para>
    ///     <para>
    ///         This allows for multiple migration configurations to coexist in the same application,
    ///         each with their own database connection and set of migrations.
    ///     </para>
    /// </remarks>
    public object? ServiceKey { get; set; }

    /// <summary>
    ///     Gets the list of assemblies to scan for migration types.
    /// </summary>
    /// <value>A list of <see cref="Assembly" /> objects to scan for types that implement <see cref="IMigration" />.</value>
    /// <remarks>
    ///     <para>
    ///         Assemblies added to this list will be scanned during service configuration to automatically
    ///         register any types that implement <see cref="IMigration" />.
    ///     </para>
    ///     <para>
    ///         Use the <see cref="ScanInAssemblies" /> method to add assemblies in a fluent manner.
    ///     </para>
    /// </remarks>
    public List<Assembly> Assemblies { get; set; } = [];

    /// <summary>
    ///     Gets the list of explicitly registered migration types.
    /// </summary>
    /// <value>A list of migration types that implement <see cref="IMigration" />.</value>
    /// <remarks>
    ///     <para>
    ///         Migration types added to this list will be registered directly with the service collection.
    ///         Use the <see cref="AddMigration{TMigration}" /> method to add migration types in a fluent manner.
    ///     </para>
    /// </remarks>
    public List<Type> MigrationTypes { get; set; } = [];

    /// <summary>
    ///     Gets or sets the database schema name for migrations.
    /// </summary>
    /// <value>The schema name as a string, or <see langword="null" /> for the default schema.</value>
    public string? Schema { get; set; }

    /// <summary>
    ///     Gets or sets the name of the migrations tracking table.
    /// </summary>
    /// <value>The table name as a string. Default is "__migrations".</value>
    /// <remarks>
    ///     <para>
    ///         This table stores information about applied migrations, including their version numbers
    ///         and the timestamp when they were applied.
    ///     </para>
    ///     <para>
    ///         The default table name is defined in <see cref="MigrationsConstants.Tables.Migrations.TableName" />.
    ///     </para>
    /// </remarks>
    public string Table { get; set; } = MigrationsConstants.Tables.Migrations.TableName;

    /// <summary>
    ///     Gets or sets the database driver for migrations.
    /// </summary>
    /// <value>A <see cref="MigrationsDriver" /> instance that provides database-specific functionality.</value>
    /// <remarks>
    ///     <para>
    ///         The driver is responsible for configuring the database-specific services (connection factory
    ///         and SQL strings) needed for migration operations.
    ///     </para>
    /// </remarks>
    public MigrationsDriver? Driver { get; set; }

    /// <summary>
    ///     Gets or sets the exception handler for errors during database connection opening.
    /// </summary>
    /// <value>
    ///     An <see cref="Action{T1, T2}" /> delegate that receives the <see cref="IServiceProvider" /> and
    ///     <see cref="Exception" /> when an error occurs during connection opening.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This handler is invoked when an exception occurs during the opening of a database connection
    ///         in the <see cref="MigrationRunner.GetOpenDbConnection" /> method.
    ///     </para>
    ///     <para>
    ///         The exception is still rethrown after the handler is invoked.
    ///     </para>
    /// </remarks>
    public Action<IServiceProvider, Exception>? OnExceptionDuringOpeningConnection { get; set; }

    /// <summary>
    ///     Gets or sets the exception handler for errors during migration up operations.
    /// </summary>
    /// <value>
    ///     An <see cref="Action{T1, T2}" /> delegate that receives the <see cref="IServiceProvider" /> and
    ///     <see cref="Exception" /> when an error occurs during <see cref="IMigrationRunner.MigrateUp" />.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This handler is invoked when an exception occurs during the execution of migration Up methods.
    ///     </para>
    ///     <para>
    ///         The exception is still rethrown after the handler is invoked.
    ///     </para>
    /// </remarks>
    public Action<IServiceProvider, Exception>? OnExceptionDuringMigrateUp { get; set; }

    /// <summary>
    ///     Gets or sets the exception handler for errors during migration down operations.
    /// </summary>
    /// <value>
    ///     An <see cref="Action{T1, T2}" /> delegate that receives the <see cref="IServiceProvider" /> and
    ///     <see cref="Exception" /> when an error occurs during <see cref="IMigrationRunner.MigrateDown" />.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This handler is invoked when an exception occurs during the execution of migration Down methods.
    ///     </para>
    ///     <para>
    ///         The exception is still rethrown after the handler is invoked.
    ///     </para>
    /// </remarks>
    public Action<IServiceProvider, Exception>? OnExceptionDuringMigrateDown { get; set; }

    /// <summary>
    ///     Gets or sets the exception handler for errors during schema and table creation.
    /// </summary>
    /// <value>
    ///     An <see cref="Action{T1, T2}" /> delegate that receives the <see cref="IServiceProvider" /> and
    ///     <see cref="Exception" /> when an error occurs during <see cref="IMigrationRunner.InitializeDatabase" />.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This handler is invoked when an exception occurs during the creation of the database schema
    ///         or migrations table.
    ///     </para>
    ///     <para>
    ///         The exception is still rethrown after the handler is invoked.
    ///     </para>
    /// </remarks>
    public Action<IServiceProvider, Exception>? OnExceptionDuringCreatingSchemaAndTable { get; set; }

    /// <summary>
    ///     Adds a migration type to the configuration.
    /// </summary>
    /// <typeparam name="TMigration">The migration type to add.</typeparam>
    /// <returns>The same <see cref="MigrationsConfiguration" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when <typeparamref name="TMigration" /> does not implement <see cref="IMigration" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method adds the specified migration type to the <see cref="MigrationTypes" /> list.
    ///         The migration will be registered with the service collection during configuration.
    ///     </para>
    /// </remarks>
    public MigrationsConfiguration AddMigration<TMigration>()
        where TMigration : class, IMigration
    {
        MigrationTypes.Add(typeof(TMigration));
        return this;
    }

    /// <summary>
    ///     Adds assemblies to scan for migration types.
    /// </summary>
    /// <param name="assemblies">The assemblies to add to the scan list.</param>
    /// <returns>The same <see cref="MigrationsConfiguration" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="assemblies" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method adds the specified assemblies to the <see cref="Assemblies" /> list.
    ///         During service configuration, these assemblies will be scanned for types that implement
    ///         <see cref="IMigration" />, and those types will be automatically registered.
    ///     </para>
    ///     <para>
    ///         Duplicate assemblies are automatically removed to ensure each assembly is only scanned once.
    ///     </para>
    /// </remarks>
    public MigrationsConfiguration ScanInAssemblies(params Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
        Assemblies = Assemblies.Distinct().ToList();

        return this;
    }
}