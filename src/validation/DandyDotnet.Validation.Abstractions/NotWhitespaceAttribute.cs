using System.ComponentModel.DataAnnotations;

namespace DandyDotnet.Validation.Abstractions;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class NotWhitespaceAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        if (value is not string stringValue)
            throw new ArgumentException("Value is not a string.", nameof(value));

        return !string.IsNullOrWhiteSpace(stringValue);
    }
}