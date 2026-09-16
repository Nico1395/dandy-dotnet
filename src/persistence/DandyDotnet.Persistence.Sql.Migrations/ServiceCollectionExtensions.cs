using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Extension methods for <see cref="IServiceCollection" /> to configure database migrations.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds database migration services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configure">An action to configure the <see cref="MigrationsConfiguration" />.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="services" /> or <paramref name="configure" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no persistence driver is configured (i.e., <see cref="MigrationsConfiguration.Driver" /> is <see langword="null" />).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method configures the database migration framework with the specified configuration.
    ///         The <paramref name="configure" /> action receives a <see cref="MigrationsConfiguration" /> instance
    ///         that can be used to configure various aspects of the migrations framework.
    ///     </para>
    ///     <para>
    ///         The method performs the following actions:
    ///         <list type="number">
    ///             <item><description>Creates and configures a <see cref="MigrationsConfiguration" /> instance</description></item>
    ///             <item><description>Validates that a database driver is configured</description></item>
    ///             <item><description>Configures database-specific services using the driver</description></item>
    ///             <item><description>Registers the <see cref="IMigrationRunner" /> implementation</description></item>
    ///             <item><description>Registers the <see cref="MigrationsConfiguration" /> instance</description></item>
    ///             <item><description>Scans configured assemblies for <see cref="IMigration" /> implementations</description></item>
    ///             <item><description>Registers explicitly configured migration types</description></item>
    ///         </list>
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
    /// <seealso cref="MigrationsConfiguration" />
    /// <seealso cref="MigrationsDriver" />
    /// <seealso cref="IMigrationRunner" />
    public static IServiceCollection AddMigrations(this IServiceCollection services, Action<MigrationsConfiguration> configure)
    {
        var configuration = new MigrationsConfiguration();
        configure(configuration);

        if (configuration.Driver == null)
            throw new InvalidOperationException("No persistence driver was configured.");

        configuration.Driver.ConfigureServices(services, configuration);

        services.AddKeyedSingletonOrDefault<IMigrationRunner, MigrationRunner>(configuration.ServiceKey, (sp, _) => new MigrationRunner(configuration, sp));
        services.AddKeyedSingletonOrDefault(configuration.ServiceKey, configuration);

        services.ScanAndAdd(scanner =>
        {
            scanner.ScanIn(configuration.Assemblies);
            scanner.ScanFor<IMigration>(migration => migration.WithKey(configuration.ServiceKey));
        });

        if (configuration.ServiceKey == null)
            services.AddTransientRange<IMigration>(configuration.MigrationTypes);
        else
            services.AddKeyedTransientRange<IMigration>(configuration.ServiceKey, configuration.MigrationTypes);

        return services;
    }
}