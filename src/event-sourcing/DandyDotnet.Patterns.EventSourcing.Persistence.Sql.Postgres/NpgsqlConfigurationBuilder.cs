namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;

/// <summary>
///     Provides a fluent builder for configuring PostgreSQL persistence options.
/// </summary>
public sealed class NpgsqlConfigurationBuilder
{
    private readonly NpgsqlConfiguration _configuration = new();

    /// <summary>
    ///     Sets the PostgreSQL database connection string.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public NpgsqlConfigurationBuilder WithConnectionString(string connectionString)
    {
        _configuration.ConnectionString = connectionString;
        return this;
    }

    internal NpgsqlConfiguration Build()
    {
        return _configuration;   
    }
}