using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

/// <summary>
///     PostgreSQL-specific implementation of <see cref="MigrationsDriver" />.
/// </summary>
/// <remarks>
///     <para>
///         This class provides PostgreSQL-specific configuration for the migrations framework.
///         It configures the PostgreSQL-specific <see cref="IDbConnectionFactory" /> and
///         <see cref="MigrationsSqlStrings" /> implementations.
///     </para>
///     <para>
///         Use the <see cref="MigrationsConfigurationExtensions.UsePostgres" /> extension method to configure
///         PostgreSQL migrations in a fluent manner.
///     </para>
/// </remarks>
/// <seealso cref="MigrationsDriver" />
/// <seealso cref="PostgresMigrationsSqlStrings" />
/// <seealso cref="MigrationsConfigurationExtensions.UsePostgres" />
internal sealed class PostgresMigrationsDriver : MigrationsDriver
{
    /// <summary>
    ///     Configures PostgreSQL-specific services for migrations.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The migrations configuration.</param>
    /// <remarks>
    ///     <para>
    ///         This method registers the PostgreSQL-specific <see cref="IDbConnectionFactory" /> and
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
            services.AddSingleton<IDbConnectionFactory>(new PostgresDbConnectionFactory(ConnectionString));
            services.AddSingleton<MigrationsSqlStrings>(new PostgresMigrationsSqlStrings(configuration));
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory>(configuration.ServiceKey, new PostgresDbConnectionFactory(ConnectionString));
            services.AddKeyedSingleton<MigrationsSqlStrings>(configuration.ServiceKey, new PostgresMigrationsSqlStrings(configuration));
        }
    }
}