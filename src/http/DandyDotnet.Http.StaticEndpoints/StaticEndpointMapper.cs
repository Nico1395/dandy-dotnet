using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DandyDotnet.Http.StaticEndpoints;

/// <summary>
///     Default implementation of <see cref="IStaticEndpointMapper" /> that discovers and maps static endpoint methods.
/// </summary>
/// <remarks>
///     <para>
///         This mapper scans assemblies for static methods marked with <see cref="HttpMethodAttribute" /> and registers
///         them as HTTP endpoints. It also supports explicitly configured methods through the
///         <see cref="StaticEndpointsConfiguration" />.
///     </para>
///     <para>
///         The mapper handles both public and non-public static methods but excludes generic methods.
///         Each discovered method is converted to a delegate and registered with the appropriate HTTP method
///         and route template from its <see cref="HttpMethodAttribute" />.
///     </para>
/// </remarks>
internal sealed class StaticEndpointMapper : IStaticEndpointMapper
{
    private readonly StaticEndpointsConfiguration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StaticEndpointMapper" /> class.
    /// </summary>
    /// <param name="configuration">The configuration containing assemblies and methods to map as endpoints.</param>
    public StaticEndpointMapper(StaticEndpointsConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    ///     Maps discovered static endpoints to the specified <see cref="WebApplication" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method discovers static endpoint methods from the configured assemblies and explicitly configured
    ///         methods, then registers them as routes in the <see cref="WebApplication" /> based on their
    ///         <see cref="HttpMethodAttribute" /> metadata.
    ///     </para>
    ///     <para>
    ///         The discovery process:
    ///         <list type="number">
    ///             <item><description>Scans all classes in configured assemblies</description></item>
    ///             <item><description>Finds static methods (public and non-public) with <see cref="HttpMethodAttribute" /></description></item>
    ///             <item><description>Excludes generic methods</description></item>
    ///             <item><description>Combines with explicitly configured methods</description></item>
    ///             <item><description>Removes duplicates by method handle</description></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Each method is registered with:
    ///         <list type="bullet">
    ///             <item><description>Route template from the attribute</description></item>
    ///             <item><description>HTTP methods from the attribute</description></item>
    ///             <item><description>Metadata from the declaring type and method (excluding HttpMethodAttribute)</description></item>
    ///         </list>
    ///     </para>
    /// </remarks>
    /// <param name="app">The <see cref="WebApplication" /> to map endpoints to.</param>
    public void MapStaticEndpoints(WebApplication app)
    {
        var endpointMethods = _configuration.Assemblies
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
            .Concat(_configuration.Methods)
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

    /// <summary>
    ///     Creates a delegate for the specified method.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method creates a delegate type based on the method's parameter types and return type,
    ///         then creates and returns a delegate instance for the method.
    ///     </para>
    /// </remarks>
    /// <param name="method">The method to create a delegate for.</param>
    /// <returns>A <see cref="Delegate" /> instance that can invoke the specified method.</returns>
    /// <exception cref="ArgumentException">Thrown when the method signature cannot be converted to a delegate.</exception>
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