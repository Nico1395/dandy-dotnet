using System.Reflection;

namespace DandyDotnet.Http.StaticEndpoints;

public sealed class StaticEndpointsConfiguration
{
    public List<Assembly> Assemblies { get; set; } = new();
    public List<MethodInfo> Methods { get; set; } = new();
}