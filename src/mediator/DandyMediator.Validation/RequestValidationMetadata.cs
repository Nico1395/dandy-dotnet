using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DandyMediator.Validation;

internal sealed class RequestValidationMetadata(bool hasValidationAttributes, IReadOnlyDictionary<PropertyInfo, ValidationAttribute[]> validationProperties)
{
    public bool HasValidationAttributes { get; } = hasValidationAttributes;
    public IReadOnlyDictionary<PropertyInfo, ValidationAttribute[]> ValidationProperties { get; } = validationProperties;
}
