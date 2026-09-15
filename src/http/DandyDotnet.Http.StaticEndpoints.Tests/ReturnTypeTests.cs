using System.Text.Json;
using DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;
using DandyDotnet.Http.StaticEndpoints.Tests.Mocks;
using Microsoft.AspNetCore.Http.Metadata;

namespace DandyDotnet.Http.StaticEndpoints.Tests;

public sealed class ReturnTypeTests
{
    [Theory]
    [InlineData("string", "hello", "text/plain", 200)]
    [InlineData("integer", "42", "application/json", 200)]
    [InlineData("boolean", "true", "application/json", 200)]
    [InlineData("object", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("record", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("array", "[1,2,3]", "application/json", 200)]
    [InlineData("null", "null", "application/json", 200)]
    [InlineData("result", "{\"name\":\"sample\",\"count\":12}", "application/json", 202)]
    [InlineData("typed-result", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("task-string", "hello", "text/plain", 200)]
    [InlineData("task-integer", "42", "application/json", 200)]
    [InlineData("task-record", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("task-object", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("task-result", "{\"name\":\"sample\",\"count\":12}", "application/json", 202)]
    [InlineData("task-typed-result", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("value-task-record", "{\"name\":\"sample\",\"count\":12}", "application/json", 200)]
    [InlineData("value-task-result", "{\"name\":\"sample\",\"count\":12}", "application/json", 202)]
    public async Task MapStaticEndpoints_ReturnType_WritesExpectedResponse(string endpoint, string body, string contentType, int status)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ReturnTypeEndpoints));
        var context = await fixture.ExecuteAsync($"/returns/{endpoint}");
        Assert.Equal(status, context.Response.StatusCode);
        Assert.StartsWith(contentType, context.Response.ContentType);
        Assert.Equal(body, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Theory]
    [InlineData("void")]
    [InlineData("task")]
    [InlineData("value-task")]
    public async Task MapStaticEndpoints_NoReturnValue_CompletesHandlerWithoutBody(string endpoint)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ReturnTypeEndpoints));
        var context = await fixture.ExecuteAsync($"/returns/{endpoint}");
        Assert.Equal(true, context.Items["completed"]);
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal("", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_NoContentResult_PreservesStatusAndEmptyBody()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ReturnTypeEndpoints));
        var context = await fixture.ExecuteAsync("/returns/no-content");
        Assert.Equal(204, context.Response.StatusCode);
        Assert.Equal("", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_RedirectResult_PreservesLocationHeader()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ReturnTypeEndpoints));
        var context = await fixture.ExecuteAsync("/returns/redirect");
        Assert.Equal(302, context.Response.StatusCode);
        Assert.Equal("/destination", context.Response.Headers.Location.ToString());
    }

    [Fact]
    public async Task MapStaticEndpoints_TaskProblemResult_PreservesProblemDetails()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ReturnTypeEndpoints));
        var context = await fixture.ExecuteAsync("/returns/problem");
        Assert.Equal(409, context.Response.StatusCode);
        Assert.StartsWith("application/problem+json", context.Response.ContentType);
        using var json = JsonDocument.Parse(await StaticEndpointsFixture.ReadBodyAsync(context));
        Assert.Equal("failure", json.RootElement.GetProperty("detail").GetString());
        Assert.Equal(409, json.RootElement.GetProperty("status").GetInt32());
    }

    [Theory]
    [InlineData("typed-result")]
    [InlineData("task-typed-result")]
    public async Task MapStaticEndpoints_TypedResult_InfersResponseMetadata(string endpoint)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ReturnTypeEndpoints));
        var metadata = fixture.GetEndpoint($"/returns/{endpoint}").Metadata.GetMetadata<IProducesResponseTypeMetadata>();
        Assert.NotNull(metadata);
        Assert.Equal(typeof(MockBody), metadata.Type);
        Assert.Equal(200, metadata.StatusCode);
        Assert.Contains("application/json", metadata.ContentTypes);
    }
}
