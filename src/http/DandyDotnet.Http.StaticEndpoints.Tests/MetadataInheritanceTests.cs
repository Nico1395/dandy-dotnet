using DandyDotnet.Http.StaticEndpoints.Tests.Fixtures;
using DandyDotnet.Http.StaticEndpoints.Tests.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Http.StaticEndpoints.Tests;

public sealed class MetadataInheritanceTests(MetadataInheritanceFixture fixture) : IClassFixture<MetadataInheritanceFixture>
{
    [Fact]
    public async Task MapStaticEndpoints_CustomMetadata_PreservesClassAndMethodAttributes()
    {
        var metadata = fixture.GetEndpoint("/metadata/inheritance").Metadata.GetOrderedMetadata<MockMetadataAttribute>();
        Assert.Contains(metadata, attribute => attribute.Value == "base");
        Assert.Contains(metadata, attribute => attribute.Value == "class");
        Assert.Contains(metadata, attribute => attribute.Value == "method-one");
        Assert.Contains(metadata, attribute => attribute.Value == "method-two");
    }

    [Fact]
    public async Task MapStaticEndpoints_MethodMetadata_TakesPrecedenceOverClassMetadata()
    {
        var metadata = fixture.GetEndpoint("/metadata/inheritance").Metadata;
        Assert.Equal(new[] { "method-tag" }, metadata.GetMetadata<TagsAttribute>()!.Tags);
        var ordered = metadata.GetOrderedMetadata<TagsAttribute>();
        Assert.Contains(ordered, attribute => attribute.Tags.Contains("class-tag"));
        Assert.Equal(new[] { "method-tag" }, ordered.Last().Tags);
    }

    [Fact]
    public async Task MapStaticEndpoints_NonInheritedAttribute_DoesNotCopyBaseClassMetadata()
    {
        Assert.Null(fixture.GetEndpoint("/metadata/inheritance").Metadata.GetMetadata<MockNonInheritedMetadataAttribute>());
    }

    [Fact]
    public async Task MapStaticEndpoints_AuthorizeAttributes_PreservesAllPolicies()
    {
        var policies = fixture.GetEndpoint("/metadata/inheritance").Metadata.GetOrderedMetadata<IAuthorizeData>().Select(attribute => attribute.Policy);
        Assert.Contains("base-policy", policies);
        Assert.Contains("class-policy", policies);
        Assert.Contains("method-policy", policies);
    }

    [Fact]
    public async Task MapStaticEndpoints_AllowAnonymous_PreservesAnonymousAndClassAuthorizationMetadata()
    {
        var metadata = fixture.GetEndpoint("/metadata/anonymous").Metadata;
        Assert.NotNull(metadata.GetMetadata<IAllowAnonymous>());
        Assert.Contains(metadata.GetOrderedMetadata<IAuthorizeData>(), attribute => attribute.Policy == "class-policy");
    }

    [Fact]
    public async Task MapStaticEndpoints_MultipleResponseAttributes_PreservesEveryDeclaration()
    {
        var metadata = fixture.GetEndpoint("/metadata/responses").Metadata.GetOrderedMetadata<ProducesResponseTypeAttribute>();
        Assert.Contains(metadata, attribute => attribute.StatusCode == 200 && attribute.Type == typeof(MockBody));
        Assert.Contains(metadata, attribute => attribute.StatusCode == 400 && attribute.Type == typeof(ProblemDetails));
        Assert.Contains(metadata, attribute => attribute.StatusCode == 404 && attribute.Type == typeof(void));
    }
}
