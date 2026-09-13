namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

public static class MigrationsConfigurationExtensions
{
    public static MigrationsConfiguration UseSqlServer(this MigrationsConfiguration configuration)
    {
        configuration.Driver = new SqlServerMigrationsDriver();
        return configuration;
    }
}