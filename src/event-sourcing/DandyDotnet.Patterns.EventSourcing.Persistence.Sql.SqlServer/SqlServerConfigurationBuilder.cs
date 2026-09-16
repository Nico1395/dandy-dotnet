namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

/// <summary>
///     Provides a fluent builder for configuring SQL Server persistence options.
/// </summary>
public sealed class SqlServerConfigurationBuilder
{
    private readonly SqlServerConfiguration _configuration = new();

    /// <summary>
    ///     Sets the SQL Server database connection string.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public SqlServerConfigurationBuilder WithConnectionString(string connectionString)
    {
        _configuration.ConnectionString = connectionString;
        return this;
    }

    internal SqlServerConfiguration Build()
    {
        return _configuration;   
    }
}