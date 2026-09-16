namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

/// <summary>
///     Extension methods for <see cref="MigrationsConfiguration" /> to configure SQL Server migrations.
/// </summary>
public static class MigrationsConfigurationExtensions
{
    /// <summary>
    ///     Configures the migrations to use SQL Server as the database provider.
    /// </summary>
    /// <param name="configuration">The migrations configuration to configure.</param>
    /// <param name="connectionString">The SQL Server connection string.</param>
    /// <returns>The same <see cref="MigrationsConfiguration" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configuration" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method sets the <see cref="MigrationsConfiguration.Driver" /> property to a
    ///         <see cref="SqlServerMigrationsDriver" /> instance configured with the specified connection string.
    ///     </para>
    ///     <para>
    ///         Use this method in the configuration action passed to <see cref="ServiceCollectionExtensions.AddMigrations" />:
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         services.AddMigrations(config => config
    ///             .UseSqlServer("Server=localhost;Database=mydb;User Id=sa;Password=password;")
    ///             .ScanInAssemblies(typeof(MyMigration).Assembly));
    ///     </code>
    /// </example>
    /// <seealso cref="MigrationsConfiguration" />
    /// <seealso cref="SqlServerMigrationsDriver" />
    /// <seealso cref="ServiceCollectionExtensions.AddMigrations" />
    public static MigrationsConfiguration UseSqlServer(this MigrationsConfiguration configuration, string? connectionString)
    {
        configuration.Driver = new SqlServerMigrationsDriver()
        {
            ConnectionString = connectionString
        };

        return configuration;
    }
}