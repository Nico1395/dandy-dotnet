using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

/// <summary>
///     SQL Server-specific implementation of <see cref="MigrationsDriver" />.
/// </summary>
/// <remarks>
///     <para>
///         This class provides SQL Server-specific configuration for the migrations framework.
///         It configures the SQL Server-specific <see cref="IDbConnectionFactory" /> and
///         <see cref="MigrationsSqlStrings" /> implementations.
///     </para>
///     <para>
///         Use the <see cref="MigrationsConfigurationExtensions.UseSqlServer" /> extension method to configure
///         SQL Server migrations in a fluent manner.
///     </para>
/// </remarks>
/// <seealso cref="MigrationsDriver" />
/// <seealso cref="SqlServerMigrationsSqlStrings" />
/// <seealso cref="MigrationsConfigurationExtensions.UseSqlServer" />
internal sealed class SqlServerMigrationsDriver : MigrationsDriver
{
    /// <summary>
    ///     Configures SQL Server-specific services for migrations.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The migrations configuration.</param>
    /// <remarks>
    ///     <para>
    ///         This method registers the SQL Server-specific <see cref="IDbConnectionFactory" /> and
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
            services.AddSingleton<IDbConnectionFactory>(new SqlServerDbConnectionFactory(ConnectionString));
            services.AddSingleton<MigrationsSqlStrings>(new SqlServerMigrationsSqlStrings(configuration));
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory>(configuration.ServiceKey, new SqlServerDbConnectionFactory(ConnectionString));
            services.AddKeyedSingleton<MigrationsSqlStrings>(configuration.ServiceKey, new SqlServerMigrationsSqlStrings(configuration));
        }
    }
}