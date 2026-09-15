using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

public sealed class MockHttpMethodsAttribute(string template) : HttpMethodAttribute(["GET", "POST"], template);

public class HttpMethodEndpoints
{
    [HttpGet("/methods/get")]
    public static string Get() => "GET";
    [HttpPost("/methods/post")]
    public static string Post() => "POST";
    [HttpPut("/methods/put")]
    public static string Put() => "PUT";
    [HttpPatch("/methods/patch")]
    public static string Patch() => "PATCH";
    [HttpDelete("/methods/delete")]
    public static string Delete() => "DELETE";
    [HttpHead("/methods/head")]
    public static string Head() => "HEAD";
    [HttpOptions("/methods/options")]
    public static string Options() => "OPTIONS";
    [MockHttpMethods("/methods/multiple")]
    public static string Multiple() => "multiple";
    [AcceptVerbs("GET", "POST", Route = "/methods/accept-verbs")]
    public static string AcceptVerbsOnly() => "ignored";
    [HttpGet]
    public static string MissingTemplate() => "ignored";
    [HttpGet("")]
    public static string EmptyTemplate() => "ignored";
    [HttpGet("   ")]
    public static string WhitespaceTemplate() => "ignored";
    [HttpGet("/methods/instance")]
    public string Instance() => "ignored";
    [HttpGet("/methods/generic")]
    public static T Generic<T>() => default!;
    public static string NoAttribute() => "ignored";
}
