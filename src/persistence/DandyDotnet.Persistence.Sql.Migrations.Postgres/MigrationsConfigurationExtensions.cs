namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

public static class MigrationsConfigurationExtensions
{
    public static MigrationsConfiguration UsePostgres(this MigrationsConfiguration configuration, string? connectionString)
    {
        configuration.Driver = new PostgresMigrationsDriver()
        {
            ConnectionString = connectionString,
        };

        return configuration;
    }
}