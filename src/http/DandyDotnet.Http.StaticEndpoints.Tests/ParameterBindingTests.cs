using System.Text;
using System.Text.Json;
using DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;
using DandyDotnet.Http.StaticEndpoints.Tests.Mocks;
using Microsoft.AspNetCore.Http;

namespace DandyDotnet.Http.StaticEndpoints.Tests;

public sealed class ParameterBindingTests
{
    [Theory]
    [InlineData("query", "?value=hello%20world", "hello world")]
    [InlineData("query-name", "?count=17&value=99", "17")]
    [InlineData("query-optional", "", "missing")]
    [InlineData("query-default", "", "42")]
    [InlineData("query-default", "?value=7", "7")]
    [InlineData("query-array", "?value=2&value=5", "[2,5]")]
    [InlineData("query-inferred", "?value=23", "23")]
    public async Task MapStaticEndpoints_QueryParameter_BindsValue(string endpoint, string query, string expected)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync($"/parameters/{endpoint}", c => c.Request.QueryString = new QueryString(query));
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(expected, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Theory]
    [InlineData("route/{value}", "value", "19", "19")]
    [InlineData("route-inferred/{value}", "value", "21", "21")]
    [InlineData("route-name/{id}", "id", "b31d21a7-8f3f-4eac-bb1b-7462f68d8dc1", "\"b31d21a7-8f3f-4eac-bb1b-7462f68d8dc1\"")]
    public async Task MapStaticEndpoints_RouteParameter_BindsValue(string endpoint, string key, string value, string expected)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync($"/parameters/{endpoint}", c =>
        {
            c.Request.RouteValues[key] = value;
            c.Request.QueryString = new QueryString($"?{key}=999");
        });
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(expected, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_FromHeader_BindsNamedHeader()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/header", c => c.Request.Headers["X-Value"] = "header-value");
        Assert.Equal("header-value", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Theory]
    [InlineData("body")]
    [InlineData("body-inferred")]
    public async Task MapStaticEndpoints_JsonBody_BindsComplexObject(string endpoint)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync($"/parameters/{endpoint}", c => SetBody(c, "{\"name\":\"sample\",\"count\":12}"));
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(new MockBody("sample", 12), JsonSerializer.Deserialize<MockBody>(await StaticEndpointsFixture.ReadBodyAsync(context), new JsonSerializerOptions(JsonSerializerDefaults.Web)));
    }

    [Fact]
    public async Task MapStaticEndpoints_OptionalBody_AllowsEmptyBody()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/body-optional");
        Assert.Equal("missing", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_FromForm_BindsNamedFormField()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/form", c => SetBody(c, "title=form+value", "application/x-www-form-urlencoded"));
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal("form value", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Theory]
    [InlineData("service", "service")]
    [InlineData("service-inferred", "service")]
    [InlineData("keyed-service", "keyed-service")]
    public async Task MapStaticEndpoints_ServiceParameter_ResolvesRegisteredInstance(string endpoint, string expected)
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync($"/parameters/{endpoint}");
        Assert.Equal(expected, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_HttpContext_ReceivesRequestContext()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/context", c => c.TraceIdentifier = "request-123");
        Assert.Same(context, context.Items["received-context"]);
        Assert.Equal("request-123", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task MapStaticEndpoints_CancellationToken_ReceivesRequestAborted(bool cancelled)
    {
        using var source = new CancellationTokenSource();
        if (cancelled) source.Cancel();
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/cancellation", c => c.RequestAborted = source.Token);
        Assert.Equal(source.Token, Assert.IsType<CancellationToken>(context.Items["received-token"]));
    }

    [Fact]
    public async Task MapStaticEndpoints_CombinedParameters_BindsEachSource()
    {
        using var source = new CancellationTokenSource();
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/combined/{id}", c =>
        {
            c.Request.RouteValues["id"] = "31";
            c.Request.QueryString = new QueryString("?count=7");
            c.Request.Headers["X-Value"] = "header";
            c.TraceIdentifier = "combined-request";
            c.RequestAborted = source.Token;
            SetBody(c, "{\"name\":\"body\",\"count\":4}");
        });
        using var json = JsonDocument.Parse(await StaticEndpointsFixture.ReadBodyAsync(context));
        var result = json.RootElement;
        Assert.Equal(31, result.GetProperty("id").GetInt32());
        Assert.Equal(7, result.GetProperty("count").GetInt32());
        Assert.Equal("header", result.GetProperty("header").GetString());
        Assert.Equal("body", result.GetProperty("body").GetProperty("name").GetString());
        Assert.Equal(4, result.GetProperty("body").GetProperty("count").GetInt32());
        Assert.Equal("service", result.GetProperty("service").GetString());
        Assert.Equal("combined-request", result.GetProperty("trace").GetString());
        Assert.True(result.GetProperty("sameToken").GetBoolean());
    }

    [Fact]
    public async Task MapStaticEndpoints_AsParameters_BindsGroupedSources()
    {
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/group/{id}", c =>
        {
            c.Request.RouteValues["id"] = "11";
            c.Request.QueryString = new QueryString("?count=8");
        });
        using var json = JsonDocument.Parse(await StaticEndpointsFixture.ReadBodyAsync(context));
        Assert.Equal(11, json.RootElement.GetProperty("id").GetInt32());
        Assert.Equal(8, json.RootElement.GetProperty("count").GetInt32());
        Assert.Equal("service", json.RootElement.GetProperty("service").GetString());
    }

    [Fact]
    public async Task MapStaticEndpoints_FromFormFile_BindsUploadedFile()
    {
        using var content = new MultipartFormDataContent("test-boundary");
        content.Add(new StringContent("file contents"), "file", "sample.txt");
        var bytes = await content.ReadAsByteArrayAsync();
        await using var fixture = new StaticEndpointsFixture(typeof(ParameterEndpoints));
        var context = await fixture.ExecuteAsync("/parameters/file", c =>
        {
            c.Request.Body = new MemoryStream(bytes);
            c.Request.ContentLength = bytes.Length;
            c.Request.ContentType = content.Headers.ContentType!.ToString();
        });
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal("sample.txt:file contents", await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    private static void SetBody(HttpContext context, string body, string contentType = "application/json")
    {
        var bytes = Encoding.UTF8.GetBytes(body);
        context.Request.Body = new MemoryStream(bytes);
        context.Request.ContentLength = bytes.Length;
        context.Request.ContentType = contentType;
    }
}
