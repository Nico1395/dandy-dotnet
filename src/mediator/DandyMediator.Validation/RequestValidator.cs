using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DandyMediator.Validation;

internal sealed class RequestValidator(
    DandyMediatorValidationPluginConfiguration configuration,
    IServiceProvider serviceProvider) : IRequestValidator
{
    public IResponseValidationResult? Validate(object request)
    {
        var metadata = RequestValidatorCache.GetOrAdd(request.GetType());
        if (!metadata.HasValidationAttributes)
            return null;

        var errors = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        ValidateProperties(errors, request, metadata.ValidationProperties);

        return errors.Count == 0 ? null : new ResponseValidationResult("Validation errors occurred", errors);
    }

    private void ValidateProperties(Dictionary<string, List<string>> errors, object item, IReadOnlyDictionary<PropertyInfo, ValidationAttribute[]> validationProperties, string? parentPath = null, int depth = 0)
    {
        if (depth > configuration.RecursionDepth)
            return;

        foreach (var (property, validationAttributes) in validationProperties)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateValue(
                property.GetValue(item),
                new ValidationContext(item, serviceProvider, items: null),
                results,
                validationAttributes);

            var path = parentPath == null ? property.Name : $"{parentPath}.{property.Name}";
            CollectErrors(results, path, errors);

            // We re-use the metadata cache for the nested properties. This should be the most performant way of checking
            // whether a complex property even needs validation.
            var value = property.GetValue(item);
            if (value == null)
                continue;

            if (value is IEnumerable enumerable and not string)
            {
                var enumerableItemType = property.PropertyType.GetGenericArguments()[0];
                var metadata = RequestValidatorCache.GetOrAdd(enumerableItemType);
                if (!metadata.HasValidationAttributes)
                    continue;

                var index = 0;
                foreach (var enumerableItem in enumerable)
                {
                    if (enumerableItem == null)
                        continue;

                    ValidateProperties(
                        errors,
                        enumerableItem,
                        metadata.ValidationProperties,
                        $"{path}.{property.Name}[{index++}]",
                        depth + 1);
                }
            }
            else
            {
                var metadata = RequestValidatorCache.GetOrAdd(property.PropertyType);
                if (!metadata.HasValidationAttributes)
                    continue;

                ValidateProperties(
                    errors,
                    value,
                    metadata.ValidationProperties,
                    $"{path}.{property.Name}",
                    depth + 1);
            }
        }
    }

    private static void CollectErrors(List<ValidationResult> results, string path, Dictionary<string, List<string>> errors)
    {
        if (results.Count == 0)
            return;

        if (!errors.ContainsKey(path))
            errors[path] = [];

        var propertyErrors = errors[path];
        propertyErrors.AddRange(results.Select(result => result.ErrorMessage ?? "Invalid value."));
    }
}
