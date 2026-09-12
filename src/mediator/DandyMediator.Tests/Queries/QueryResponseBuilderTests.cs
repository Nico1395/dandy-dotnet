using DandyMediator.Queries;
using DandyMediator.Responses;

namespace DandyMediator.Tests.Queries;

public class QueryResponseBuilderTests
{
    [Fact]
    public void Create_OK200()
    {
        var response = QueryResponse.OK_200(true).Build();

        Assert.Equal(RequestResponseStatus.OK_200, response.Status);
        Assert.True(response.Data);
    }

    [Fact]
    public void Create_Created201()
    {
        var response = QueryResponse.Created_201<bool>().Build();

        Assert.Equal(RequestResponseStatus.Created_201, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_Accepted202()
    {
        var response = QueryResponse.Accepted_202<bool>().Build();

        Assert.Equal(RequestResponseStatus.Accepted_202, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_NoContent204()
    {
        var response = QueryResponse.NoContent_204<bool>().Build();

        Assert.Equal(RequestResponseStatus.NoContent_204, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_BadRequest400()
    {
        var response = QueryResponse.BadRequest_400<bool>().Build();

        Assert.Equal(RequestResponseStatus.BadRequest_400, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_Unauthorized401()
    {
        var response = QueryResponse.Unauthorized_401<bool>().Build();

        Assert.Equal(RequestResponseStatus.Unauthorized_401, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_Forbidden403()
    {
        var response = QueryResponse.Forbidden_403<bool>().Build();

        Assert.Equal(RequestResponseStatus.Forbidden_403, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_NotFound404()
    {
        var response = QueryResponse.NotFound_404<bool>().Build();

        Assert.Equal(RequestResponseStatus.NotFound_404, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_NotAcceptable406()
    {
        var response = QueryResponse.NotAcceptable_406<bool>().Build();

        Assert.Equal(RequestResponseStatus.NotAcceptable_406, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_Conflict409()
    {
        var response = QueryResponse.Conflict_409<bool>().Build();

        Assert.Equal(RequestResponseStatus.Conflict_409, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_UnprocessableEntity422()
    {
        var response = QueryResponse.UnprocessableEntity_422<bool>().Build();

        Assert.Equal(RequestResponseStatus.UnprocessableEntity_422, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_InternalServerError500()
    {
        var response = QueryResponse.InternalServerError_500<bool>().Build();

        Assert.Equal(RequestResponseStatus.InternalServerError_500, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_NotImplemented501()
    {
        var response = QueryResponse.NotImplemented_501<bool>().Build();

        Assert.Equal(RequestResponseStatus.NotImplemented_501, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void Create_ServiceUnavailable503()
    {
        var response = QueryResponse.ServiceUnavailable_503<bool>().Build();

        Assert.Equal(RequestResponseStatus.ServiceUnavailable_503, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithMetadata_AddsMetadata()
    {
        var response = QueryResponse.OK_200(true).WithMetadata("key", "value").Build();

        Assert.Equal("value", response.GetMetadataValue("key"));
    }
}