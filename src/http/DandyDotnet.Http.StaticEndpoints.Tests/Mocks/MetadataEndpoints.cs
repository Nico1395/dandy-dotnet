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

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

[RequestTimeout("timeout-policy")]
public static class RequestTimeoutClassEndpoints
{
    [HttpGet("/metadata/RequestTimeout/Class")]
    public static string Execute() => "metadata";
}

public static class RequestTimeoutMethodEndpoints
{
    [RequestTimeout("timeout-policy")]
    [HttpGet("/metadata/RequestTimeout/Method")]
    public static string Execute() => "metadata";
}

[DisableRequestTimeout]
public static class DisableRequestTimeoutClassEndpoints
{
    [HttpGet("/metadata/DisableRequestTimeout/Class")]
    public static string Execute() => "metadata";
}

public static class DisableRequestTimeoutMethodEndpoints
{
    [DisableRequestTimeout]
    [HttpGet("/metadata/DisableRequestTimeout/Method")]
    public static string Execute() => "metadata";
}

[AllowCookieRedirect]
public static class AllowCookieRedirectClassEndpoints
{
    [HttpGet("/metadata/AllowCookieRedirect/Class")]
    public static string Execute() => "metadata";
}

public static class AllowCookieRedirectMethodEndpoints
{
    [AllowCookieRedirect]
    [HttpGet("/metadata/AllowCookieRedirect/Method")]
    public static string Execute() => "metadata";
}

[Authorize(Policy = "policy", Roles = "admin", AuthenticationSchemes = "scheme")]
public static class AuthorizeClassEndpoints
{
    [HttpGet("/metadata/Authorize/Class")]
    public static string Execute() => "metadata";
}

public static class AuthorizeMethodEndpoints
{
    [Authorize(Policy = "policy", Roles = "admin", AuthenticationSchemes = "scheme")]
    [HttpGet("/metadata/Authorize/Method")]
    public static string Execute() => "metadata";
}

[AllowAnonymous]
public static class AllowAnonymousClassEndpoints
{
    [HttpGet("/metadata/AllowAnonymous/Class")]
    public static string Execute() => "metadata";
}

public static class AllowAnonymousMethodEndpoints
{
    [AllowAnonymous]
    [HttpGet("/metadata/AllowAnonymous/Method")]
    public static string Execute() => "metadata";
}

[Tags("one", "two")]
public static class TagsClassEndpoints
{
    [HttpGet("/metadata/Tags/Class")]
    public static string Execute() => "metadata";
}

public static class TagsMethodEndpoints
{
    [Tags("one", "two")]
    [HttpGet("/metadata/Tags/Method")]
    public static string Execute() => "metadata";
}

public static class EndpointSummaryMethodEndpoints
{
    [EndpointSummary("summary")]
    [HttpGet("/metadata/EndpointSummary/Method")]
    public static string Execute() => "metadata";
}

public static class EndpointDescriptionMethodEndpoints
{
    [EndpointDescription("description")]
    [HttpGet("/metadata/EndpointDescription/Method")]
    public static string Execute() => "metadata";
}

public static class EndpointNameMethodEndpoints
{
    [EndpointName("named-endpoint")]
    [HttpGet("/metadata/EndpointName/Method")]
    public static string Execute() => "metadata";
}

[ExcludeFromDescription]
public static class ExcludeFromDescriptionClassEndpoints
{
    [HttpGet("/metadata/ExcludeFromDescription/Class")]
    public static string Execute() => "metadata";
}

public static class ExcludeFromDescriptionMethodEndpoints
{
    [ExcludeFromDescription]
    [HttpGet("/metadata/ExcludeFromDescription/Method")]
    public static string Execute() => "metadata";
}

[ProducesResponseType(typeof(MockBody), 201, "application/json")]
public static class ProducesResponseTypeClassEndpoints
{
    [HttpGet("/metadata/ProducesResponseType/Class")]
    public static string Execute() => "metadata";
}

public static class ProducesResponseTypeMethodEndpoints
{
    [ProducesResponseType(typeof(MockBody), 201, "application/json")]
    [HttpGet("/metadata/ProducesResponseType/Method")]
    public static string Execute() => "metadata";
}

[Produces("application/xml")]
public static class ProducesClassEndpoints
{
    [HttpGet("/metadata/Produces/Class")]
    public static string Execute() => "metadata";
}

public static class ProducesMethodEndpoints
{
    [Produces("application/xml")]
    [HttpGet("/metadata/Produces/Method")]
    public static string Execute() => "metadata";
}

[ProducesErrorResponseType(typeof(ProblemDetails))]
public static class ProducesErrorResponseTypeClassEndpoints
{
    [HttpGet("/metadata/ProducesErrorResponseType/Class")]
    public static string Execute() => "metadata";
}

public static class ProducesErrorResponseTypeMethodEndpoints
{
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    [HttpGet("/metadata/ProducesErrorResponseType/Method")]
    public static string Execute() => "metadata";
}

public static class ProducesDefaultResponseTypeMethodEndpoints
{
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    [HttpGet("/metadata/ProducesDefaultResponseType/Method")]
    public static string Execute() => "metadata";
}

[Consumes("application/json")]
public static class ConsumesClassEndpoints
{
    [HttpGet("/metadata/Consumes/Class")]
    public static string Execute() => "metadata";
}

public static class ConsumesMethodEndpoints
{
    [Consumes("application/json")]
    [HttpGet("/metadata/Consumes/Method")]
    public static string Execute() => "metadata";
}

[ResponseCache(Duration = 60, NoStore = true)]
public static class ResponseCacheClassEndpoints
{
    [HttpGet("/metadata/ResponseCache/Class")]
    public static string Execute() => "metadata";
}

public static class ResponseCacheMethodEndpoints
{
    [ResponseCache(Duration = 60, NoStore = true)]
    [HttpGet("/metadata/ResponseCache/Method")]
    public static string Execute() => "metadata";
}

[OutputCache(PolicyName = "cache", Duration = 30, Tags = new[] { "tag" })]
public static class OutputCacheClassEndpoints
{
    [HttpGet("/metadata/OutputCache/Class")]
    public static string Execute() => "metadata";
}

public static class OutputCacheMethodEndpoints
{
    [OutputCache(PolicyName = "cache", Duration = 30, Tags = new[] { "tag" })]
    [HttpGet("/metadata/OutputCache/Method")]
    public static string Execute() => "metadata";
}

[EnableCors("cors")]
public static class EnableCorsClassEndpoints
{
    [HttpGet("/metadata/EnableCors/Class")]
    public static string Execute() => "metadata";
}

public static class EnableCorsMethodEndpoints
{
    [EnableCors("cors")]
    [HttpGet("/metadata/EnableCors/Method")]
    public static string Execute() => "metadata";
}

[DisableCors]
public static class DisableCorsClassEndpoints
{
    [HttpGet("/metadata/DisableCors/Class")]
    public static string Execute() => "metadata";
}

public static class DisableCorsMethodEndpoints
{
    [DisableCors]
    [HttpGet("/metadata/DisableCors/Method")]
    public static string Execute() => "metadata";
}

[EnableRateLimiting("rate")]
public static class EnableRateLimitingClassEndpoints
{
    [HttpGet("/metadata/EnableRateLimiting/Class")]
    public static string Execute() => "metadata";
}

public static class EnableRateLimitingMethodEndpoints
{
    [EnableRateLimiting("rate")]
    [HttpGet("/metadata/EnableRateLimiting/Method")]
    public static string Execute() => "metadata";
}

[DisableRateLimiting]
public static class DisableRateLimitingClassEndpoints
{
    [HttpGet("/metadata/DisableRateLimiting/Class")]
    public static string Execute() => "metadata";
}

public static class DisableRateLimitingMethodEndpoints
{
    [DisableRateLimiting]
    [HttpGet("/metadata/DisableRateLimiting/Method")]
    public static string Execute() => "metadata";
}

[RequestSizeLimit(1024)]
public static class RequestSizeLimitClassEndpoints
{
    [HttpGet("/metadata/RequestSizeLimit/Class")]
    public static string Execute() => "metadata";
}

public static class RequestSizeLimitMethodEndpoints
{
    [RequestSizeLimit(1024)]
    [HttpGet("/metadata/RequestSizeLimit/Method")]
    public static string Execute() => "metadata";
}

[DisableRequestSizeLimit]
public static class DisableRequestSizeLimitClassEndpoints
{
    [HttpGet("/metadata/DisableRequestSizeLimit/Class")]
    public static string Execute() => "metadata";
}

public static class DisableRequestSizeLimitMethodEndpoints
{
    [DisableRequestSizeLimit]
    [HttpGet("/metadata/DisableRequestSizeLimit/Method")]
    public static string Execute() => "metadata";
}

[RequestFormLimits(ValueCountLimit = 17)]
public static class RequestFormLimitsClassEndpoints
{
    [HttpGet("/metadata/RequestFormLimits/Class")]
    public static string Execute() => "metadata";
}

public static class RequestFormLimitsMethodEndpoints
{
    [RequestFormLimits(ValueCountLimit = 17)]
    [HttpGet("/metadata/RequestFormLimits/Method")]
    public static string Execute() => "metadata";
}

[IgnoreAntiforgeryToken]
public static class IgnoreAntiforgeryTokenClassEndpoints
{
    [HttpGet("/metadata/IgnoreAntiforgeryToken/Class")]
    public static string Execute() => "metadata";
}

public static class IgnoreAntiforgeryTokenMethodEndpoints
{
    [IgnoreAntiforgeryToken]
    [HttpGet("/metadata/IgnoreAntiforgeryToken/Method")]
    public static string Execute() => "metadata";
}

[RequireAntiforgeryToken(false)]
public static class RequireAntiforgeryTokenClassEndpoints
{
    [HttpGet("/metadata/RequireAntiforgeryToken/Class")]
    public static string Execute() => "metadata";
}

public static class RequireAntiforgeryTokenMethodEndpoints
{
    [RequireAntiforgeryToken(false)]
    [HttpGet("/metadata/RequireAntiforgeryToken/Method")]
    public static string Execute() => "metadata";
}

[DisableHttpMetrics]
public static class DisableHttpMetricsClassEndpoints
{
    [HttpGet("/metadata/DisableHttpMetrics/Class")]
    public static string Execute() => "metadata";
}

public static class DisableHttpMetricsMethodEndpoints
{
    [DisableHttpMetrics]
    [HttpGet("/metadata/DisableHttpMetrics/Method")]
    public static string Execute() => "metadata";
}

[HttpLogging(HttpLoggingFields.RequestPath)]
public static class HttpLoggingClassEndpoints
{
    [HttpGet("/metadata/HttpLogging/Class")]
    public static string Execute() => "metadata";
}

public static class HttpLoggingMethodEndpoints
{
    [HttpLogging(HttpLoggingFields.RequestPath)]
    [HttpGet("/metadata/HttpLogging/Method")]
    public static string Execute() => "metadata";
}

[ApiExplorerSettings(GroupName = "group", IgnoreApi = true)]
public static class ApiExplorerSettingsClassEndpoints
{
    [HttpGet("/metadata/ApiExplorerSettings/Class")]
    public static string Execute() => "metadata";
}

public static class ApiExplorerSettingsMethodEndpoints
{
    [ApiExplorerSettings(GroupName = "group", IgnoreApi = true)]
    [HttpGet("/metadata/ApiExplorerSettings/Method")]
    public static string Execute() => "metadata";
}

[Obsolete("legacy")]
public static class ObsoleteClassEndpoints
{
    [HttpGet("/metadata/Obsolete/Class")]
    public static string Execute() => "metadata";
}

public static class ObsoleteMethodEndpoints
{
    [Obsolete("legacy")]
    [HttpGet("/metadata/Obsolete/Method")]
    public static string Execute() => "metadata";
}

