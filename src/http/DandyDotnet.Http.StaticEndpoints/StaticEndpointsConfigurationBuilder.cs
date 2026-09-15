using System.Reflection;

namespace DandyDotnet.Http.StaticEndpoints;

public sealed class StaticEndpointsConfigurationBuilder
{
    private readonly StaticEndpointsConfiguration _configuration = new();

    public StaticEndpointsConfigurationBuilder ScanInAssemblies(params IEnumerable<Assembly> assemblies)
    {
        _configuration.Assemblies.AddRange(assemblies);
        return this;
    }

    public StaticEndpointsConfiguration Build()
    {
        return _configuration;
    }
}