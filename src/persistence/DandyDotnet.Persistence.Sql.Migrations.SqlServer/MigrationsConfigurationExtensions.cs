namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

public static class MigrationsConfigurationExtensions
{
    public static MigrationsConfiguration UseSqlServer(this MigrationsConfiguration configuration, string? connectionString)
    {
        configuration.Driver = new SqlServerMigrationsDriver()
        {
            ConnectionString = connectionString
        };

        return configuration;
    }
}