using DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;
using Microsoft.AspNetCore.Routing;

namespace DandyDotnet.Http.StaticEndpoints.Tests;

public sealed class HttpMethodAttributeTests(StaticEndpointsFixture fixture) : IClassFixture<StaticEndpointsFixture>
{
    [Theory]
    [InlineData("get", "GET")]
    [InlineData("post", "POST")]
    [InlineData("put", "PUT")]
    [InlineData("patch", "PATCH")]
    [InlineData("delete", "DELETE")]
    [InlineData("head", "HEAD")]
    [InlineData("options", "OPTIONS")]
    public async Task MapStaticEndpoints_HttpMethodAttribute_MapsRouteAndVerb(string endpoint, string verb)
    {
        var route = $"/methods/{endpoint}";
        Assert.Equal(new[] { verb }, fixture.GetEndpoint(route).Metadata.GetMetadata<HttpMethodMetadata>()!.HttpMethods);
        var context = await fixture.ExecuteAsync(route);
        Assert.Equal(verb, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_CustomHttpMethodAttribute_MapsAllVerbs()
    {
        Assert.Equal(new[] { "GET", "POST" }, fixture.GetEndpoint("/methods/multiple").Metadata.GetMetadata<HttpMethodMetadata>()!.HttpMethods);
    }

    [Fact]
    public async Task MapStaticEndpoints_AcceptVerbsWithoutHttpMethodAttribute_DoesNotMapEndpoint()
    {
        // AcceptVerbsAttribute does not derive from HttpMethodAttribute, which is
        // the framework's discovery contract.
        Assert.DoesNotContain(fixture.Endpoints, endpoint => endpoint.RoutePattern.RawText == "/methods/accept-verbs");
    }

    [Fact]
    public async Task MapStaticEndpoints_AssemblyScanning_IgnoresIneligibleMethods()
    {
        var endpoints = fixture.Endpoints.Where(endpoint => endpoint.RoutePattern.RawText!.StartsWith("/methods/")).ToArray();
        Assert.Equal(8, endpoints.Length);
        Assert.DoesNotContain(fixture.Endpoints, endpoint => string.IsNullOrWhiteSpace(endpoint.RoutePattern.RawText));
        Assert.DoesNotContain(endpoints, endpoint => endpoint.RoutePattern.RawText is "/methods/instance" or "/methods/generic");
    }
}
