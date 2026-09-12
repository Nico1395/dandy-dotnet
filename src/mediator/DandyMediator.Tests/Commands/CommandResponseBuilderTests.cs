using DandyMediator.Commands;
using DandyMediator.Responses;

namespace DandyMediator.Tests.Commands;

public class CommandResponseBuilderTests
{
    [Fact]
    public void Create_OK200()
    {
        var response = CommandResponse.OK_200().Build();

        Assert.Equal(RequestResponseStatus.OK_200, response.Status);
    }

    [Fact]
    public void Create_Created201()
    {
        var response = CommandResponse.Created_201().Build();

        Assert.Equal(RequestResponseStatus.Created_201, response.Status);
    }

    [Fact]
    public void Create_Accepted202()
    {
        var response = CommandResponse.Accepted_202().Build();

        Assert.Equal(RequestResponseStatus.Accepted_202, response.Status);
    }

    [Fact]
    public void Create_NoContent204()
    {
        var response = CommandResponse.NoContent_204().Build();

        Assert.Equal(RequestResponseStatus.NoContent_204, response.Status);
    }

    [Fact]
    public void Create_BadRequest400()
    {
        var response = CommandResponse.BadRequest_400().Build();

        Assert.Equal(RequestResponseStatus.BadRequest_400, response.Status);
    }

    [Fact]
    public void Create_Unauthorized401()
    {
        var response = CommandResponse.Unauthorized_401().Build();

        Assert.Equal(RequestResponseStatus.Unauthorized_401, response.Status);
    }

    [Fact]
    public void Create_Forbidden403()
    {
        var response = CommandResponse.Forbidden_403().Build();

        Assert.Equal(RequestResponseStatus.Forbidden_403, response.Status);
    }

    [Fact]
    public void Create_NotFound404()
    {
        var response = CommandResponse.NotFound_404().Build();

        Assert.Equal(RequestResponseStatus.NotFound_404, response.Status);
    }

    [Fact]
    public void Create_NotAcceptable406()
    {
        var response = CommandResponse.NotAcceptable_406().Build();

        Assert.Equal(RequestResponseStatus.NotAcceptable_406, response.Status);
    }

    [Fact]
    public void Create_Conflict409()
    {
        var response = CommandResponse.Conflict_409().Build();

        Assert.Equal(RequestResponseStatus.Conflict_409, response.Status);
    }

    [Fact]
    public void Create_UnprocessableEntity422()
    {
        var response = CommandResponse.UnprocessableEntity_422().Build();

        Assert.Equal(RequestResponseStatus.UnprocessableEntity_422, response.Status);
    }

    [Fact]
    public void Create_InternalServerError500()
    {
        var response = CommandResponse.InternalServerError_500().Build();

        Assert.Equal(RequestResponseStatus.InternalServerError_500, response.Status);
    }

    [Fact]
    public void Create_NotImplemented501()
    {
        var response = CommandResponse.NotImplemented_501().Build();

        Assert.Equal(RequestResponseStatus.NotImplemented_501, response.Status);
    }

    [Fact]
    public void Create_ServiceUnavailable503()
    {
        var response = CommandResponse.ServiceUnavailable_503().Build();

        Assert.Equal(RequestResponseStatus.ServiceUnavailable_503, response.Status);
    }

    [Fact]
    public void WithMetadata_AddsMetadata()
    {
        var response = CommandResponse.OK_200().WithMetadata("key", "value").Build();

        Assert.Equal("value", response.GetMetadataValue("key"));
    }
    
    [Fact]
    public void WithData_Create_OK200()
    {
        var response = CommandResponse.OK_200(true).Build();

        Assert.Equal(RequestResponseStatus.OK_200, response.Status);
        Assert.True(response.Data);
    }

    [Fact]
    public void WithData_Create_Created201()
    {
        var response = CommandResponse.Created_201<bool>().Build();

        Assert.Equal(RequestResponseStatus.Created_201, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_Accepted202()
    {
        var response = CommandResponse.Accepted_202<bool>().Build();

        Assert.Equal(RequestResponseStatus.Accepted_202, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_NoContent204()
    {
        var response = CommandResponse.NoContent_204<bool>().Build();

        Assert.Equal(RequestResponseStatus.NoContent_204, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_BadRequest400()
    {
        var response = CommandResponse.BadRequest_400<bool>().Build();

        Assert.Equal(RequestResponseStatus.BadRequest_400, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_Unauthorized401()
    {
        var response = CommandResponse.Unauthorized_401<bool>().Build();

        Assert.Equal(RequestResponseStatus.Unauthorized_401, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_Forbidden403()
    {
        var response = CommandResponse.Forbidden_403<bool>().Build();

        Assert.Equal(RequestResponseStatus.Forbidden_403, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_NotFound404()
    {
        var response = CommandResponse.NotFound_404<bool>().Build();

        Assert.Equal(RequestResponseStatus.NotFound_404, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_NotAcceptable406()
    {
        var response = CommandResponse.NotAcceptable_406<bool>().Build();

        Assert.Equal(RequestResponseStatus.NotAcceptable_406, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_Conflict409()
    {
        var response = CommandResponse.Conflict_409<bool>().Build();

        Assert.Equal(RequestResponseStatus.Conflict_409, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_UnprocessableEntity422()
    {
        var response = CommandResponse.UnprocessableEntity_422<bool>().Build();

        Assert.Equal(RequestResponseStatus.UnprocessableEntity_422, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_InternalServerError500()
    {
        var response = CommandResponse.InternalServerError_500<bool>().Build();

        Assert.Equal(RequestResponseStatus.InternalServerError_500, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_NotImplemented501()
    {
        var response = CommandResponse.NotImplemented_501<bool>().Build();

        Assert.Equal(RequestResponseStatus.NotImplemented_501, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_Create_ServiceUnavailable503()
    {
        var response = CommandResponse.ServiceUnavailable_503<bool>().Build();

        Assert.Equal(RequestResponseStatus.ServiceUnavailable_503, response.Status);
        Assert.False(response.Data);
    }

    [Fact]
    public void WithData_WithMetadata_AddsMetadata()
    {
        var response = CommandResponse.OK_200(true).WithMetadata("key", "value").Build();

        Assert.Equal("value", response.GetMetadataValue("key"));
    }
}