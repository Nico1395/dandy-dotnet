using System.Reflection;

namespace DandyDotnet.Http.Server.Endpoints;

public sealed class EndpointsConfigurationBuilder
{
    private readonly EndpointsConfiguration _configuration = new();

    public EndpointsConfigurationBuilder ScanInAssemblies(params Assembly[] assemblies)
    {
        _configuration.Assemblies = assemblies;
        return this;
    }

    public EndpointsConfiguration Build()
    {
        return _configuration;
    }
}