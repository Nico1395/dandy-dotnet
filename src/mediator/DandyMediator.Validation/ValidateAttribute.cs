using System.ComponentModel.DataAnnotations;

namespace DandyMediator.Validation;

/// <summary>
/// Used to mark a property or parameter to be validated during request validation.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class ValidateAttribute : ValidationAttribute
{
    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        return true;
    }
}