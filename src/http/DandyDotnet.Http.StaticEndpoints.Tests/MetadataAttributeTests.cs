using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing;
using DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;
using DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

namespace DandyDotnet.Http.StaticEndpoints.Tests;

public sealed class MetadataAttributeTests(StaticEndpointsFixture fixture) : IClassFixture<StaticEndpointsFixture>
{
    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_RequestTimeoutAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/RequestTimeout/{scope}").Metadata.GetMetadata<RequestTimeoutAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("timeout-policy", metadata.PolicyName);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_DisableRequestTimeoutAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/DisableRequestTimeout/{scope}").Metadata.GetMetadata<DisableRequestTimeoutAttribute>();
        Assert.NotNull(metadata);
        Assert.IsType<DisableRequestTimeoutAttribute>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_AllowCookieRedirectAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/AllowCookieRedirect/{scope}").Metadata.GetMetadata<AllowCookieRedirectAttribute>();
        Assert.NotNull(metadata);
        Assert.IsType<AllowCookieRedirectAttribute>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_AuthorizeAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/Authorize/{scope}").Metadata.GetMetadata<AuthorizeAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("policy", metadata.Policy);
        Assert.Equal("admin", metadata.Roles);
        Assert.Equal("scheme", metadata.AuthenticationSchemes);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_AllowAnonymousAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/AllowAnonymous/{scope}").Metadata.GetMetadata<AllowAnonymousAttribute>();
        Assert.NotNull(metadata);
        Assert.IsAssignableFrom<IAllowAnonymous>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_TagsAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/Tags/{scope}").Metadata.GetMetadata<TagsAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(new[] { "one", "two" }, metadata.Tags);
    }

    [Theory]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_EndpointSummaryAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/EndpointSummary/{scope}").Metadata.GetMetadata<EndpointSummaryAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("summary", metadata.Summary);
    }

    [Theory]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_EndpointDescriptionAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/EndpointDescription/{scope}").Metadata.GetMetadata<EndpointDescriptionAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("description", metadata.Description);
    }

    [Theory]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_EndpointNameAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/EndpointName/{scope}").Metadata.GetMetadata<EndpointNameAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("named-endpoint", metadata.EndpointName);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ExcludeFromDescriptionAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/ExcludeFromDescription/{scope}").Metadata.GetMetadata<ExcludeFromDescriptionAttribute>();
        Assert.NotNull(metadata);
        Assert.True(metadata.ExcludeFromDescription);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ProducesResponseTypeAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/ProducesResponseType/{scope}").Metadata.GetMetadata<ProducesResponseTypeAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(typeof(MockBody), metadata.Type);
        Assert.Equal(201, metadata.StatusCode);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ProducesAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/Produces/{scope}").Metadata.GetMetadata<ProducesAttribute>();
        Assert.NotNull(metadata);
        Assert.Contains("application/xml", metadata.ContentTypes);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ProducesErrorResponseTypeAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/ProducesErrorResponseType/{scope}").Metadata.GetMetadata<ProducesErrorResponseTypeAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(typeof(ProblemDetails), metadata.Type);
    }

    [Theory]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ProducesDefaultResponseTypeAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/ProducesDefaultResponseType/{scope}").Metadata.GetMetadata<ProducesDefaultResponseTypeAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(typeof(ProblemDetails), metadata.Type);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ConsumesAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/Consumes/{scope}").Metadata.GetMetadata<ConsumesAttribute>();
        Assert.NotNull(metadata);
        Assert.Contains("application/json", metadata.ContentTypes);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ResponseCacheAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/ResponseCache/{scope}").Metadata.GetMetadata<ResponseCacheAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(60, metadata.Duration);
        Assert.True(metadata.NoStore);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_OutputCacheAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/OutputCache/{scope}").Metadata.GetMetadata<OutputCacheAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("cache", metadata.PolicyName);
        Assert.Equal(30, metadata.Duration);
        Assert.Equal(new[] { "tag" }, metadata.Tags);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_EnableCorsAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/EnableCors/{scope}").Metadata.GetMetadata<EnableCorsAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("cors", metadata.PolicyName);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_DisableCorsAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/DisableCors/{scope}").Metadata.GetMetadata<DisableCorsAttribute>();
        Assert.NotNull(metadata);
        Assert.IsType<DisableCorsAttribute>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_EnableRateLimitingAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/EnableRateLimiting/{scope}").Metadata.GetMetadata<EnableRateLimitingAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("rate", metadata.PolicyName);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_DisableRateLimitingAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/DisableRateLimiting/{scope}").Metadata.GetMetadata<DisableRateLimitingAttribute>();
        Assert.NotNull(metadata);
        Assert.IsType<DisableRateLimitingAttribute>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_RequestSizeLimitAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/RequestSizeLimit/{scope}").Metadata.GetMetadata<RequestSizeLimitAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(1024, ((IRequestSizeLimitMetadata)metadata).MaxRequestBodySize);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_DisableRequestSizeLimitAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/DisableRequestSizeLimit/{scope}").Metadata.GetMetadata<DisableRequestSizeLimitAttribute>();
        Assert.NotNull(metadata);
        Assert.Null(((IRequestSizeLimitMetadata)metadata).MaxRequestBodySize);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_RequestFormLimitsAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/RequestFormLimits/{scope}").Metadata.GetMetadata<RequestFormLimitsAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(17, metadata.ValueCountLimit);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_IgnoreAntiforgeryTokenAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/IgnoreAntiforgeryToken/{scope}").Metadata.GetMetadata<IgnoreAntiforgeryTokenAttribute>();
        Assert.NotNull(metadata);
        Assert.IsType<IgnoreAntiforgeryTokenAttribute>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_RequireAntiforgeryTokenAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/RequireAntiforgeryToken/{scope}").Metadata.GetMetadata<RequireAntiforgeryTokenAttribute>();
        Assert.NotNull(metadata);
        Assert.False(metadata.RequiresValidation);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_DisableHttpMetricsAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/DisableHttpMetrics/{scope}").Metadata.GetMetadata<DisableHttpMetricsAttribute>();
        Assert.NotNull(metadata);
        Assert.IsType<DisableHttpMetricsAttribute>(metadata);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_HttpLoggingAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/HttpLogging/{scope}").Metadata.GetMetadata<HttpLoggingAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal(HttpLoggingFields.RequestPath, metadata.LoggingFields);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ApiExplorerSettingsAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/ApiExplorerSettings/{scope}").Metadata.GetMetadata<ApiExplorerSettingsAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("group", metadata.GroupName);
        Assert.True(metadata.IgnoreApi);
    }

    [Theory]
    [InlineData("Class")]
    [InlineData("Method")]
    public async Task MapStaticEndpoints_ObsoleteAttribute_PreservesMetadata(string scope)
    {
        var metadata = fixture.GetEndpoint($"/metadata/Obsolete/{scope}").Metadata.GetMetadata<ObsoleteAttribute>();
        Assert.NotNull(metadata);
        Assert.Equal("legacy", metadata.Message);
    }

}
