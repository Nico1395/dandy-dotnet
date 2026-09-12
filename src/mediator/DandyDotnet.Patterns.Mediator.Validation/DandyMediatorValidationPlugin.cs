using DandyMediator.Configuration;

namespace DandyDotnet.Patterns.Mediator.Validation;

internal sealed class DandyMediatorValidationPlugin : DandyMediatorPlugin
{
    public override string Key => DandyMediatorConstants.Plugins.Validation.Key;
    public override string Slot => DandyMediatorConstants.Plugins.Validation.Slot;
}
