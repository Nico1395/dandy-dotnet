using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Http.StaticEndpoints.Tests.Mocks;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class MockMetadataAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MockNonInheritedMetadataAttribute : Attribute;

[MockMetadata("base")]
[MockNonInheritedMetadata]
[Authorize("base-policy")]
public class MetadataBase;

[MockMetadata("class")]
[Tags("class-tag")]
[Authorize("class-policy")]
public class MetadataInheritanceEndpoints : MetadataBase
{
    [HttpGet("/metadata/inheritance")]
    [MockMetadata("method-one")]
    [MockMetadata("method-two")]
    [Tags("method-tag")]
    [Authorize("method-policy")]
    public static string Execute() => "metadata";

    [HttpGet("/metadata/anonymous")]
    [AllowAnonymous]
    public static string Anonymous() => "anonymous";

    [HttpGet("/metadata/responses")]
    [ProducesResponseType(typeof(MockBody), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(404)]
    public static MockBody Responses() => new("sample", 12);
}
