using System.Reflection;

namespace DandyStrategies;

public sealed class StrategiesConfiguration
{
    internal StrategiesConfiguration()
    {
    }

    public IReadOnlyList<Assembly> Assemblies { get; internal set; } = [];
}
