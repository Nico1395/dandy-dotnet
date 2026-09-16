using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

/// <summary>
///     SQLite-specific implementation of <see cref="MigrationsDriver" />.
/// </summary>
/// <remarks>
///     <para>
///         This class provides SQLite-specific configuration for the migrations framework.
///         It configures the SQLite-specific <see cref="IDbConnectionFactory" /> and
///         <see cref="MigrationsSqlStrings" /> implementations.
///     </para>
///     <para>
///         Use the <see cref="MigrationsConfigurationExtensions.UseSqlite" /> extension method to configure
///         SQLite migrations in a fluent manner.
///     </para>
/// </remarks>
/// <seealso cref="MigrationsDriver" />
/// <seealso cref="SqliteMigrationsSqlStrings" />
/// <seealso cref="MigrationsConfigurationExtensions.UseSqlite" />
internal sealed class SqliteMigrationsDriver : MigrationsDriver
{
    /// <summary>
    ///     Configures SQLite-specific services for migrations.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The migrations configuration.</param>
    /// <remarks>
    ///     <para>
    ///         This method registers the SQLite-specific <see cref="IDbConnectionFactory" /> and
    ///         <see cref="MigrationsSqlStrings" /> implementations with the service collection.
    ///     </para>
    ///     <para>
    ///         The services are registered either as non-keyed or keyed services based on the
    ///         <see cref="MigrationsConfiguration.ServiceKey" /> property.
    ///     </para>
    /// </remarks>
    public override void ConfigureServices(IServiceCollection services, MigrationsConfiguration configuration)
    {
        if (configuration.ServiceKey == null)
        {
            services.AddSingleton<IDbConnectionFactory>(new SqliteDbConnectionFactory(ConnectionString));
            services.AddSingleton<MigrationsSqlStrings>(new SqliteMigrationsSqlStrings(configuration));
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory>(configuration.ServiceKey, new SqliteDbConnectionFactory(ConnectionString));
            services.AddKeyedSingleton<MigrationsSqlStrings>(configuration.ServiceKey, new SqliteMigrationsSqlStrings(configuration));
        }
    }
}