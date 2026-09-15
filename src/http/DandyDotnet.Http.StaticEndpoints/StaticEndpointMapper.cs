using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DandyDotnet.Http.StaticEndpoints;

internal sealed class StaticEndpointMapper(StaticEndpointsConfiguration configuration) : IStaticEndpointMapper
{
    public void MapStaticEndpoints(WebApplication app)
    {
        var endpointMethods = configuration.Assemblies
            .Distinct()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is
            {
                // Excluding abstract types would exclude static classes, so don't filter them out
                IsClass: true,
            })
            .SelectMany(t => t
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m =>
                    !m.IsGenericMethod &&   // Exclude generic methods
                    m.GetCustomAttribute<HttpMethodAttribute>() != null))   // Has to have an HttpMethodAttribute
            .Concat(configuration.Methods)
            .DistinctBy(m => m.MethodHandle);

        foreach (var endpointMethod in endpointMethods)
        {
            var attribute = endpointMethod.GetCustomAttribute<HttpMethodAttribute>();
            if (attribute == null || string.IsNullOrWhiteSpace(attribute.Template))
                continue;

            var @delegate = CreateDelegate(endpointMethod);
            var endpoint = app.MapMethods(attribute.Template, attribute.HttpMethods, @delegate);
            var metadata = endpointMethod.DeclaringType?
                .GetCustomAttributes(inherit: true)
                .Concat(endpointMethod.GetCustomAttributes(inherit: true))
                .Where(a => a is not HttpMethodAttribute)
                .ToArray();

            if (metadata is { Length: > 0 })
                endpoint.WithMetadata(metadata);
        }
    }

    private static Delegate CreateDelegate(MethodInfo method)
    {
        var typeArguments = method
            .GetParameters()
            .Select(parameter => parameter.ParameterType)
            .Append(method.ReturnType)
            .ToArray();
        
        var delegateType = Expression.GetDelegateType(typeArguments);
        return method.CreateDelegate(delegateType);
    }
}