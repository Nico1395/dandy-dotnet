using DandyMediator.Queries;
using DandyMediator.Responses;

namespace DandyMediator.Tests.Queries;

public class QueryResponseExtensionsTests
{
    [Fact]
    public void Map_MapsResponse()
    {
        var response = QueryResponse.InternalServerError_500<bool>().Build();
        var mapped = response.Map(Convert.ToInt32);

        Assert.Equal(response.Status, mapped.Status);
    }

    [Fact]
    public void Map_OK200_WithData()
    {
        var response = QueryResponse.OK_200(true).Build();
        var mapped = response.Map(Convert.ToInt32);

        Assert.Equal(response.Status, mapped.Status);
        Assert.Equal(1, mapped.Data);
    }

    [Fact]
    public void Map_OK200_WithoutData_MapsToNoContent204()
    {
        var response = QueryResponse.OK_200<bool?>(null).Build();
        var mapped = response.Map(c => c.HasValue ? (int?)Convert.ToInt32(c.Value) : null);

        Assert.Equal(RequestResponseStatus.NoContent_204, mapped.Status);
        Assert.Null(mapped.Data);
    }
}