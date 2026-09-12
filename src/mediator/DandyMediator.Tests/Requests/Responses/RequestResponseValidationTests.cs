using DandyMediator.Responses;

namespace DandyMediator.Tests.Requests.Responses;

public class RequestResponseValidationTests
{
    [Fact]
    public void WasValidRequest()
    {
        var response = new RequestResponse(RequestResponseStatus.Created_201);

        Assert.True(response.WasValidRequest());
        Assert.False(response.WasInvalidRequest());
    }

    [Fact]
    public void WasInvalidRequest()
    {
        var response = new RequestResponse(RequestResponseStatus.UnprocessableEntity_422);

        Assert.False(response.WasValidRequest());
        Assert.True(response.WasInvalidRequest());
    }
}