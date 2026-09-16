namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

/// <summary>
///     Extension methods for <see cref="MigrationsConfiguration" /> to configure SQLite migrations.
/// </summary>
public static class MigrationsConfigurationExtensions
{
    /// <summary>
    ///     Configures the migrations to use SQLite as the database provider.
    /// </summary>
    /// <param name="configuration">The migrations configuration to configure.</param>
    /// <param name="connectionString">The SQLite connection string.</param>
    /// <returns>The same <see cref="MigrationsConfiguration" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configuration" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method sets the <see cref="MigrationsConfiguration.Driver" /> property to a
    ///         <see cref="SqliteMigrationsDriver" /> instance configured with the specified connection string.
    ///     </para>
    ///     <para>
    ///         Use this method in the configuration action passed to <see cref="ServiceCollectionExtensions.AddMigrations" />:
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         services.AddMigrations(config => config
    ///             .UseSqlite("Data Source=./mydatabase.db;")
    ///             .ScanInAssemblies(typeof(MyMigration).Assembly));
    ///     </code>
    /// </example>
    /// <seealso cref="MigrationsConfiguration" />
    /// <seealso cref="SqliteMigrationsDriver" />
    /// <seealso cref="ServiceCollectionExtensions.AddMigrations" />
    public static MigrationsConfiguration UseSqlite(this MigrationsConfiguration configuration, string? connectionString)
    {
        configuration.Driver = new SqliteMigrationsDriver()
        {
            ConnectionString = connectionString
        };

        return configuration;
    }
}