using DandyMediator.Responses;

namespace DandyMediator.Tests.Requests.Responses;

public class RequestResponseStatusTests
{
    [Fact]
    public void IsSuccess_2xx()
    {
        var response = new RequestResponse(RequestResponseStatus.OK_200);

        Assert.True(response.IsOK_200());
        Assert.True(response.IsSuccess_2xx());
    }
    
    [Fact]
    public void IsCreated_201()
    {
        var response = new RequestResponse(RequestResponseStatus.Created_201);

        Assert.True(response.IsCreated_201());
        Assert.True(response.IsSuccess_2xx());
    }
    
    [Fact]
    public void IsAccepted_202()
    {
        var response = new RequestResponse(RequestResponseStatus.Accepted_202);

        Assert.True(response.IsAccepted_202());
        Assert.True(response.IsSuccess_2xx());
    }
    
    [Fact]
    public void IsNoContent_204()
    {
        var response = new RequestResponse(RequestResponseStatus.NoContent_204);

        Assert.True(response.IsNoContent_204());
        Assert.True(response.IsSuccess_2xx());
    }
    
    [Fact]
    public void IsBadRequest_400()
    {
        var response = new RequestResponse(RequestResponseStatus.BadRequest_400);

        Assert.True(response.IsBadRequest_400());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsUnauthorized_401()
    {
        var response = new RequestResponse(RequestResponseStatus.Unauthorized_401);

        Assert.True(response.IsUnauthorized_401());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsForbidden_403()
    {
        var response = new RequestResponse(RequestResponseStatus.Forbidden_403);

        Assert.True(response.IsForbidden_403());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsNotFound_404()
    {
        var response = new RequestResponse(RequestResponseStatus.NotFound_404);

        Assert.True(response.IsNotFound_404());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsNotAcceptable_406()
    {
        var response = new RequestResponse(RequestResponseStatus.NotAcceptable_406);

        Assert.True(response.IsNotAcceptable_406());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsConflict_409()
    {
        var response = new RequestResponse(RequestResponseStatus.Conflict_409);

        Assert.True(response.IsConflict_409());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsUnprocessableEntity_422()
    {
        var response = new RequestResponse(RequestResponseStatus.UnprocessableEntity_422);

        Assert.True(response.IsUnprocessableEntity_422());
        Assert.True(response.IsClientSide_4xx());
    }
    
    [Fact]
    public void IsInternalServerError_500()
    {
        var response = new RequestResponse(RequestResponseStatus.InternalServerError_500);

        Assert.True(response.IsInternalServerError_500());
        Assert.True(response.IsServerSide_5xx());
    }
    
    [Fact]
    public void IsNotImplemented_501()
    {
        var response = new RequestResponse(RequestResponseStatus.NotImplemented_501);

        Assert.True(response.IsNotImplemented_501());
        Assert.True(response.IsServerSide_5xx());
    }
    
    [Fact]
    public void IsServiceUnavailable_503()
    {
        var response = new RequestResponse(RequestResponseStatus.ServiceUnavailable_503);

        Assert.True(response.IsServiceUnavailable_503());
        Assert.True(response.IsServerSide_5xx());
    }
}