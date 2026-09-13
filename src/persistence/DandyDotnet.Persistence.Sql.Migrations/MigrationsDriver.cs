using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations;

public abstract class MigrationsDriver
{
    public string? ConnectionString { get; set; }

    public abstract void ConfigureServices(IServiceCollection services, MigrationsConfiguration configuration);
}