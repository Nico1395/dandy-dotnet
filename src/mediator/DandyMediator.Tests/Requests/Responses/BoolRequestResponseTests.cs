using DandyMediator.Responses;

namespace DandyMediator.Tests.Requests.Responses;

public class BoolRequestResponseTests
{
    [Fact]
    public void ResultedInTrue()
    {
        var response = new RequestResponse<bool>(RequestResponseStatus.OK_200, metadata: null, true);

        Assert.True(response.ResultedInTrue());
        Assert.True(response.ResultedIn(true));
    }

    [Fact]
    public void ResultedInFalse()
    {
        var response = new RequestResponse<bool>(RequestResponseStatus.OK_200, metadata: null, false);

        Assert.True(response.ResultedInFalse());
        Assert.True(response.ResultedIn(false));
    }
}