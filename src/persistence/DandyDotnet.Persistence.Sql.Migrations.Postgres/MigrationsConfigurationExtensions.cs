namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

/// <summary>
///     Extension methods for <see cref="MigrationsConfiguration" /> to configure PostgreSQL migrations.
/// </summary>
public static class MigrationsConfigurationExtensions
{
    /// <summary>
    ///     Configures the migrations to use PostgreSQL as the database provider.
    /// </summary>
    /// <param name="configuration">The migrations configuration to configure.</param>
    /// <param name="connectionString">The PostgreSQL connection string.</param>
    /// <returns>The same <see cref="MigrationsConfiguration" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configuration" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method sets the <see cref="MigrationsConfiguration.Driver" /> property to a
    ///         <see cref="PostgresMigrationsDriver" /> instance configured with the specified connection string.
    ///     </para>
    ///     <para>
    ///         Use this method in the configuration action passed to <see cref="ServiceCollectionExtensions.AddMigrations" />:
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         services.AddMigrations(config => config
    ///             .UsePostgres("Server=localhost;Database=mydb;User Id=postgres;Password=password;")
    ///             .ScanInAssemblies(typeof(MyMigration).Assembly));
    ///     </code>
    /// </example>
    /// <seealso cref="MigrationsConfiguration" />
    /// <seealso cref="PostgresMigrationsDriver" />
    /// <seealso cref="ServiceCollectionExtensions.AddMigrations" />
    public static MigrationsConfiguration UsePostgres(this MigrationsConfiguration configuration, string? connectionString)
    {
        configuration.Driver = new PostgresMigrationsDriver()
        {
            ConnectionString = connectionString,
        };

        return configuration;
    }
}