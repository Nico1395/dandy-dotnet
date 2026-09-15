using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

public sealed record MockService(string Value);
public sealed record MockBody(string Name, int Count);

public sealed class MockParameterGroup
{
    [FromRoute] public int Id { get; set; }
    [FromQuery(Name = "count")] public int Count { get; set; }
    [FromServices] public MockService Service { get; set; } = null!;
}

public static class ParameterEndpoints
{
    [HttpGet("/parameters/group/{id}")]
    public static object Group([AsParameters] MockParameterGroup parameters) =>
        new { parameters.Id, parameters.Count, service = parameters.Service.Value };
    [HttpPost("/parameters/file")]
    public static async Task<string> File([FromForm] IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        return $"{file.FileName}:{await reader.ReadToEndAsync()}";
    }
    [HttpGet("/parameters/query")]
    public static string Query([FromQuery] string value) => value;
    [HttpGet("/parameters/query-name")]
    public static int NamedQuery([FromQuery(Name = "count")] int value) => value;
    [HttpGet("/parameters/query-optional")]
    public static string OptionalQuery([FromQuery] string? value) => value ?? "missing";
    [HttpGet("/parameters/query-default")]
    public static int DefaultQuery([FromQuery] int value = 42) => value;
    [HttpGet("/parameters/query-array")]
    public static int[] ArrayQuery([FromQuery] int[] value) => value;
    [HttpGet("/parameters/query-inferred")]
    public static int InferredQuery(int value) => value;
    [HttpGet("/parameters/route/{value}")]
    public static int Route([FromRoute] int value) => value;
    [HttpGet("/parameters/route-name/{id}")]
    public static Guid NamedRoute([FromRoute(Name = "id")] Guid value) => value;
    [HttpGet("/parameters/route-inferred/{value}")]
    public static int InferredRoute(int value) => value;
    [HttpGet("/parameters/header")]
    public static string Header([FromHeader(Name = "X-Value")] string value) => value;
    [HttpPost("/parameters/body")]
    public static MockBody Body([FromBody] MockBody value) => value;
    [HttpPost("/parameters/body-inferred")]
    public static MockBody InferredBody(MockBody value) => value;
    [HttpPost("/parameters/body-optional")]
    public static string OptionalBody([FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)] MockBody? value) => value?.Name ?? "missing";
    [HttpPost("/parameters/form")]
    public static string Form([FromForm(Name = "title")] string value) => value;
    [HttpGet("/parameters/service")]
    public static string Service([FromServices] MockService service) => service.Value;
    [HttpGet("/parameters/keyed-service")]
    public static string KeyedService([FromKeyedServices("selected")] MockService service) => service.Value;
    [HttpGet("/parameters/service-inferred")]
    public static string InferredService(MockService service) => service.Value;
    [HttpGet("/parameters/context")]
    public static string Context(HttpContext context)
    {
        context.Items["received-context"] = context;
        return context.TraceIdentifier;
    }
    [HttpGet("/parameters/cancellation")]
    public static bool Cancellation(CancellationToken cancellationToken, HttpContext context)
    {
        context.Items["received-token"] = cancellationToken;
        return cancellationToken.IsCancellationRequested;
    }
    [HttpPost("/parameters/combined/{id}")]
    public static object Combined([FromRoute] int id, [FromQuery] int count,
        [FromHeader(Name = "X-Value")] string header, [FromBody] MockBody body,
        [FromServices] MockService service, HttpContext context, CancellationToken cancellationToken) =>
        new { id, count, header, body, service = service.Value, trace = context.TraceIdentifier,
            sameToken = cancellationToken == context.RequestAborted };
}
