using DandyMediator.Commands;
using DandyMediator.Responses;

namespace DandyMediator.Tests.Commands;

public class CommandResponseExtensionsTests
{
    [Fact]
    public void WithData_Map_MapsResponse()
    {
        var response = CommandResponse.InternalServerError_500<bool>().Build();
        var mapped = response.Map(Convert.ToInt32);

        Assert.Equal(response.Status, mapped.Status);
    }

    [Fact]
    public void WithData_Map_OK200_WithData()
    {
        var response = CommandResponse.OK_200(true).Build();
        var mapped = response.Map(Convert.ToInt32);

        Assert.Equal(response.Status, mapped.Status);
        Assert.Equal(1, mapped.Data);
    }

    [Fact]
    public void WithData_Map_OK200_WithoutData_MapsToNoContent204()
    {
        var response = CommandResponse.OK_200<bool?>(null).Build();
        var mapped = response.Map(c => c.HasValue ? (int?)Convert.ToInt32(c.Value) : null);

        Assert.Equal(RequestResponseStatus.NoContent_204, mapped.Status);
        Assert.Null(mapped.Data);
    }
}