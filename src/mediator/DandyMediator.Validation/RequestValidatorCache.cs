using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DandyMediator.Validation;

internal static class RequestValidatorCache
{
    private static readonly ConcurrentDictionary<Type, RequestValidationMetadata> _cache = new();

    public static RequestValidationMetadata GetOrAdd(Type type)
    {
        return _cache.GetOrAdd(type, CreateMetadata);
    }

    private static RequestValidationMetadata CreateMetadata(Type type)
    {
        // The attributes of parameters of records would not be queried with GetCustomAttributes, so we need to query constructor parameters.

        var constructorParameters = type
            .GetConstructors()
            .SelectMany(c => c.GetParameters())
            .Where(p => p.Name != null && p.GetCustomAttributes<ValidationAttribute>(true).Any())
            .DistinctBy(p => p.Name)
            .ToDictionary(p => p.Name!, p => p.GetCustomAttributes<ValidationAttribute>(true));

        var validationProperties = new Dictionary<PropertyInfo, ValidationAttribute[]>();
        foreach (var property in type.GetProperties().Where(p => constructorParameters.ContainsKey(p.Name) || p.GetCustomAttributes<ValidationAttribute>(true).Any()))
        {
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
            if (constructorParameters.TryGetValue(property.Name, out var constructorAttributes))
                validationAttributes = validationAttributes.Concat(constructorAttributes);

            validationProperties[property] = validationAttributes.ToArray();
        }

        return new RequestValidationMetadata(
            validationProperties.Count > 0,
            validationProperties
        );
    }
}
