using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.Server.Endpoints;

public static class WebApplicationExtensions
{
    public static void MapDandyEndpoints(this WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<EndpointsConfiguration>();
        if (configuration.Assemblies != null)
            ScanEndpoints(app, configuration.Assemblies);
    }

    private static void ScanEndpoints(WebApplication app, Assembly[] assemblies)
    {
        var endpointMethods = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is
            {
                IsClass: true,
                IsGenericTypeDefinition: false,
            })
            .SelectMany(t => t
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m =>
                    m.GetCustomAttribute<HttpMethodAttribute>() != null))
            .ToArray();
        
        if (endpointMethods.Length == 0)
            return;

        foreach (var endpointMethod in endpointMethods)
        {
            var attribute = endpointMethod.GetCustomAttribute<HttpMethodAttribute>();
            if (attribute == null || string.IsNullOrWhiteSpace(attribute.Template))
                continue;

            var @delegate = CreateDelegate(endpointMethod);
            app.MapMethods(attribute.Template, attribute.HttpMethods, @delegate);
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
