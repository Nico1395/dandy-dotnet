namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

public static class MigrationsConfigurationExtensions
{
    public static MigrationsConfiguration UseSqlite(this MigrationsConfiguration configuration)
    {
        configuration.Driver = new SqliteMigrationsDriver();
        return configuration;
    }
}