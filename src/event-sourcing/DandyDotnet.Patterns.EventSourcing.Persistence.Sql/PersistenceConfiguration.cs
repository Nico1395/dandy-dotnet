using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

public abstract class PersistenceConfiguration : PluginConfiguration
{
    public string? ConnectionString { get; set; }

    public override string Slot => "persistence";

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddFluentMigratorCore();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}