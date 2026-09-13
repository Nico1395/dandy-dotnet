namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

public static class MigrationsConfigurationExtensions
{
    public static MigrationsConfiguration UsePostgres(this MigrationsConfiguration configuration)
    {
        configuration.Driver = new PostgresMigrationsDriver();
        return configuration;
    }
}