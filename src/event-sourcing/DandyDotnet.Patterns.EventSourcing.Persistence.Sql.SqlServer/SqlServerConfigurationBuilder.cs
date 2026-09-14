namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

public sealed class SqlServerConfigurationBuilder
{
    private readonly SqlServerConfiguration _configuration = new();

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