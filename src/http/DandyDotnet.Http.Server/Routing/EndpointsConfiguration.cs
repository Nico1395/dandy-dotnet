using System.Reflection;

namespace DandyDotnet.Http.Server.Endpoints;

public sealed class EndpointsConfiguration
{
    public Assembly[]? Assemblies { get; set; }
}