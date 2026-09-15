using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;

public class StaticEndpointsFixture : IAsyncDisposable
{
    private readonly WebApplication _app;
    public IReadOnlyList<RouteEndpoint> Endpoints { get; }

    public StaticEndpointsFixture() : this(null) { }

    protected StaticEndpointsFixture(Type? endpointType)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.Services.AddSingleton(new MockService("service"));
        builder.Services.AddKeyedSingleton("selected", new MockService("keyed-service"));
        builder.Services.AddStaticEndpoints(configuration =>
        {
            if (endpointType == null)
                configuration.ScanInAssemblies(typeof(StaticEndpointsFixture).Assembly);
            else
                configuration.Build().Methods.AddRange(endpointType.GetMethods(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
        });
        _app = builder.Build();
        _app.MapStaticEndpoints();
        Endpoints = ((IEndpointRouteBuilder)_app).DataSources
            .SelectMany(source => source.Endpoints).Cast<RouteEndpoint>().ToArray();
    }

    public RouteEndpoint GetEndpoint(string route) =>
        Assert.Single(Endpoints, endpoint => endpoint.RoutePattern.RawText == route);

    // Execute ASP.NET Core's generated delegate with a fresh request scope. Route values
    // are supplied explicitly because this fixture does not run routing middleware.
    public async Task<DefaultHttpContext> ExecuteAsync(string route, Action<DefaultHttpContext>? configure = null)
    {
        await using var scope = _app.Services.CreateAsyncScope();
        var endpoint = GetEndpoint(route);
        var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
        context.Request.Method = endpoint.Metadata.GetMetadata<HttpMethodMetadata>()!.HttpMethods[0];
        context.Request.Path = route;
        context.Response.Body = new MemoryStream();
        context.SetEndpoint(endpoint);
        configure?.Invoke(context);
        if (context.Request.ContentLength > 0)
            context.Features.Set<IHttpRequestBodyDetectionFeature>(new BodyDetectionFeature());
        await endpoint.RequestDelegate!(context);
        context.Response.Body.Position = 0;
        return context;
    }

    public static Task<string> ReadBodyAsync(HttpContext context) =>
        new StreamReader(context.Response.Body, leaveOpen: true).ReadToEndAsync();

    public ValueTask DisposeAsync() => _app.DisposeAsync();

    private sealed class BodyDetectionFeature : IHttpRequestBodyDetectionFeature
    {
        public bool CanHaveBody => true;
    }
}

public sealed class ParameterBindingFixture : StaticEndpointsFixture
{
    public ParameterBindingFixture() : base(typeof(ParameterEndpoints)) { }
}

public sealed class ReturnTypeFixture : StaticEndpointsFixture
{
    public ReturnTypeFixture() : base(typeof(ReturnTypeEndpoints)) { }
}

public sealed class MetadataInheritanceFixture : StaticEndpointsFixture
{
    public MetadataInheritanceFixture() : base(typeof(MetadataInheritanceEndpoints)) { }
}
