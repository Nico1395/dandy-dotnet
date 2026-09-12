using System.Collections.Generic;
using System.Reflection;

namespace DandyDotnet.Patterns.Strategies;

public sealed class StrategiesConfiguration
{
    internal StrategiesConfiguration()
    {
    }

    public IReadOnlyList<Assembly> Assemblies { get; internal set; } = [];
}
