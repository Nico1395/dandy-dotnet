namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

public static class MigrationsConfigurationExtensions
{
    public static MigrationsConfiguration UseSqlite(this MigrationsConfiguration configuration, string? connectionString)
    {
        configuration.Driver = new SqliteMigrationsDriver()
        {
            ConnectionString = connectionString
        };

        return configuration;
    }
}