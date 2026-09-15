using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

public static class ReturnTypeEndpoints
{
    [HttpGet("/returns/string")]
    public static string String() => "hello";
    [HttpGet("/returns/integer")]
    public static int Integer() => 42;
    [HttpGet("/returns/boolean")]
    public static bool Boolean() => true;
    [HttpGet("/returns/object")]
    public static object Object() => new MockBody("sample", 12);
    [HttpGet("/returns/record")]
    public static MockBody Record() => new("sample", 12);
    [HttpGet("/returns/array")]
    public static int[] Array() => [1, 2, 3];
    [HttpGet("/returns/null")]
    public static MockBody? Null() => null;
    [HttpGet("/returns/result")]
    public static IResult Result() => Results.Json(new MockBody("sample", 12), statusCode: 202);
    [HttpGet("/returns/typed-result")]
    public static Ok<MockBody> TypedResult() => TypedResults.Ok(new MockBody("sample", 12));
    [HttpGet("/returns/task-string")]
    public static async Task<string> TaskString() { await Task.Yield(); return "hello"; }
    [HttpGet("/returns/task-integer")]
    public static async Task<int> TaskInteger() { await Task.Yield(); return 42; }
    [HttpGet("/returns/task-record")]
    public static async Task<MockBody> TaskRecord() { await Task.Yield(); return new("sample", 12); }
    [HttpGet("/returns/task-object")]
    public static async Task<object> TaskObject() { await Task.Yield(); return new MockBody("sample", 12); }
    [HttpGet("/returns/task-result")]
    public static async Task<IResult> TaskResult() { await Task.Yield(); return Results.Json(new MockBody("sample", 12), statusCode: 202); }
    [HttpGet("/returns/task-typed-result")]
    public static async Task<Ok<MockBody>> TaskTypedResult() { await Task.Yield(); return TypedResults.Ok(new MockBody("sample", 12)); }
    [HttpGet("/returns/value-task-record")]
    public static async ValueTask<MockBody> ValueTaskRecord() { await Task.Yield(); return new("sample", 12); }
    [HttpGet("/returns/value-task-result")]
    public static async ValueTask<IResult> ValueTaskResult() { await Task.Yield(); return Results.Json(new MockBody("sample", 12), statusCode: 202); }
    [HttpGet("/returns/void")]
    public static void Void(HttpContext context) => context.Items["completed"] = true;
    [HttpGet("/returns/task")]
    public static async Task EmptyTask(HttpContext context) { await Task.Yield(); context.Items["completed"] = true; }
    [HttpGet("/returns/value-task")]
    public static async ValueTask EmptyValueTask(HttpContext context) { await Task.Yield(); context.Items["completed"] = true; }
    [HttpGet("/returns/no-content")]
    public static IResult NoContent() => Results.NoContent();
    [HttpGet("/returns/redirect")]
    public static IResult Redirect() => Results.Redirect("/destination");
    [HttpGet("/returns/problem")]
    public static Task<IResult> Problem() => Task.FromResult(Results.Problem("failure", statusCode: 409));
}
