using DandyMediator.Configuration;

namespace DandyDotnet.Patterns.Mediator.Validation;

internal sealed class DandyMediatorValidationPluginConfiguration : DandyMediatorPluginConfiguration
{
    public bool Enabled { get; internal set; } = true;
    public int RecursionDepth { get; internal set; } = 10;
}
