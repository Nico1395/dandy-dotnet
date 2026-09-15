using DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;

namespace DandyDotnet.Http.StaticEndpoints.Tests;

public sealed class AccessModifierTests(StaticEndpointsFixture fixture) : IClassFixture<StaticEndpointsFixture>
{
    public static IEnumerable<object[]> AccessModifiers()
    {
        string[] modifiers = ["public", "internal", "private", "protected", "protected-internal", "private-protected"];
        foreach (var container in new[] { "public", "internal" })
        foreach (var declaringClass in modifiers.Append("top-level"))
        foreach (var endpoint in modifiers)
            yield return [$"/access/{container}/{declaringClass}/{endpoint}"];
    }

    [Theory]
    [MemberData(nameof(AccessModifiers))]
    public async Task MapStaticEndpoints_ClassAndMethodAccessModifiers_MapsExecutableEndpoint(string route)
    {
        var context = await fixture.ExecuteAsync(route);
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(route, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    public static IEnumerable<object[]> StaticClassAccessModifiers()
    {
        string[] modifiers = ["public", "internal", "private", "protected", "protected-internal", "private-protected", "top-level"];
        foreach (var container in new[] { "public", "internal" })
        foreach (var declaringClass in modifiers)
        foreach (var endpoint in new[] { "public", "internal", "private" })
            yield return [$"/static-access/{container}/{declaringClass}/{endpoint}"];
    }

    [Theory]
    [MemberData(nameof(StaticClassAccessModifiers))]
    public async Task MapStaticEndpoints_StaticClassAndMethodAccessModifiers_MapsExecutableEndpoint(string route)
    {
        var context = await fixture.ExecuteAsync(route);
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(route, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Theory]
    [InlineData("regular", "public")]
    [InlineData("regular", "internal")]
    [InlineData("regular", "private")]
    [InlineData("regular", "protected")]
    [InlineData("regular", "protected-internal")]
    [InlineData("regular", "private-protected")]
    [InlineData("static", "public")]
    [InlineData("static", "internal")]
    [InlineData("static", "private")]
    public async Task MapStaticEndpoints_FileLocalClassAndMethodAccessModifiers_MapsExecutableEndpoint(string kind, string modifier)
    {
        var route = $"/file-access/{kind}/{modifier}";
        var context = await fixture.ExecuteAsync(route);
        Assert.Equal(route, await StaticEndpointsFixture.ReadBodyAsync(context));
    }

    [Fact]
    public async Task MapStaticEndpoints_FileLocalClass_MapsExecutableEndpoint()
    {
        var context = await fixture.ExecuteAsync("/access/file");
        Assert.Equal("file", await StaticEndpointsFixture.ReadBodyAsync(context));
    }
}
