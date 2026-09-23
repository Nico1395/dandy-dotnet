using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Abstract base class for database-specific migration drivers.
/// </summary>
/// <remarks>
///     <para>
///         Each database provider (PostgreSQL, SQL Server, SQLite) implements this class to provide
///         database-specific functionality for the migrations framework.
///     </para>
///     <para>
///         The driver is responsible for configuring database-specific services such as the
///         <see cref="IDbConnectionFactory" /> and <see cref="MigrationsSqlStrings" /> during service
///         collection configuration.
///     </para>
/// </remarks>
public abstract class MigrationsDriver
{
    /// <summary>
    ///     Gets or sets the database connection string.
    /// </summary>
    /// <value>The connection string as a string, or <see langword="null" /> if not configured.</value>
    /// <remarks>
    ///     <para>
    ///         The connection string is used by the database-specific connection factory to create
    ///         connections to the database.
    ///     </para>
    ///     <para>
    ///         The format of the connection string depends on the database provider.
    ///     </para>
    /// </remarks>
    public string? ConnectionString { get; set; }

    /// <summary>
    ///     Configures the services for the database driver.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The migrations configuration.</param>
    /// <remarks>
    ///     <para>
    ///         Implement this method to register database-specific services with the service collection.
    ///         This typically includes the <see cref="IDbConnectionFactory" /> and <see cref="MigrationsSqlStrings" />
    ///         implementations for the specific database provider.
    ///     </para>
    ///     <para>
    ///         The method should respect the <see cref="MigrationsConfiguration.ServiceKey" /> to determine
    ///         whether to use keyed or non-keyed service registration.
    ///     </para>
    /// </remarks>
    public abstract void ConfigureServices(IServiceCollection services, MigrationsConfiguration configuration);
}