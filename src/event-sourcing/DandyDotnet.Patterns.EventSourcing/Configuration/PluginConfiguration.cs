using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

public abstract class PluginConfiguration
{
    public abstract string Slot { get; }

    public abstract void ConfigureServices(IServiceCollection services);
}