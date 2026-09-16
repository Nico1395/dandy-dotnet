namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;

/// <summary>
///     Provides a fluent builder for configuring SQLite persistence options.
/// </summary>
public sealed class SqliteConfigurationBuilder
{
    private readonly SqliteConfiguration _configuration = new();

    /// <summary>
    ///     Sets the SQLite database connection string.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public SqliteConfigurationBuilder WithConnectionString(string connectionString)
    {
        _configuration.ConnectionString = connectionString;
        return this;
    }

    internal SqliteConfiguration Build()
    {
        return _configuration;   
    }
}