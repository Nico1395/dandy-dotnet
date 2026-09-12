using DandyMediator.Queries;
using DandyMediator.Responses;

namespace DandyMediator.Tests.Queries;

public class QueryResponseFactoryMethodTests
{
    [Fact]
    public void OkOrNotFound_CreatesOK200()
    {
        var response = QueryResponse.OkOrNotFound(true).Build();

        Assert.Equal(RequestResponseStatus.OK_200, response.Status);
        Assert.True(response.Data);
    }

    [Fact]
    public void OkOrNotFound_CreatesNotFound404()
    {
        var response = QueryResponse.OkOrNotFound<bool?>(null).Build();

        Assert.Equal(RequestResponseStatus.NotFound_404, response.Status);
        Assert.Null(response.Data);
    }

    [Fact]
    public void FromData_CreatesOK200()
    {
        var response = QueryResponse.FromData(true);

        Assert.Equal(RequestResponseStatus.OK_200, response.Status);
        Assert.True(response.Data);
    }

    [Fact]
    public void FromData_CreatesNotFound404()
    {
        var response = QueryResponse.FromData<bool?>(null);

        Assert.Equal(RequestResponseStatus.NotFound_404, response.Status);
        Assert.Null(response.Data);
    }

    [Fact]
    public void ToResponse_CreatesOK200()
    {
        var data = true;
        var response = data.ToResponse();

        Assert.Equal(RequestResponseStatus.OK_200, response.Status);
        Assert.True(response.Data);
    }

    [Fact]
    public void ToResponse_CreatesNotFound404()
    {
        bool? data = null;
        var response = data.ToResponse();

        Assert.Equal(RequestResponseStatus.NotFound_404, response.Status);
        Assert.Null(response.Data);
    }
}