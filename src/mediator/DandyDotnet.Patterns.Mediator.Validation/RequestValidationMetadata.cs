using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DandyDotnet.Patterns.Mediator.Validation;

internal sealed class RequestValidationMetadata(bool hasValidationAttributes, IReadOnlyDictionary<PropertyInfo, ValidationAttribute[]> validationProperties)
{
    public bool HasValidationAttributes { get; } = hasValidationAttributes;
    public IReadOnlyDictionary<PropertyInfo, ValidationAttribute[]> ValidationProperties { get; } = validationProperties;
}
