using System.Collections.Generic;
using System.Reflection;

namespace DandyDotnet.Patterns.Strategies;

/// <summary>
///     Represents configuration options for strategy registration and discovery.
/// </summary>
public sealed class StrategiesConfiguration
{
    internal StrategiesConfiguration()
    {
    }

    /// <summary>
    ///     Gets the collection of assemblies scanned or configured for strategy implementations.
    /// </summary>
    public IReadOnlyList<Assembly> Assemblies { get; internal set; } = [];
}
