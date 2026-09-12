using DandyMediator.Responses;

namespace DandyMediator.Tests.Requests.Responses;

public class RequestResponseMetadataTests
{
    [Fact]
    public void TryGetMetadata_ReturnsTrue()
    {
        var metadata = new Dictionary<string, object> { { "key", "value" } };
        var response = new RequestResponse(RequestResponseStatus.OK_200, metadata);

        Assert.True(response.TryGetMetadata("key", out var value));
        Assert.Equal("value", value);
    }

    [Fact]
    public void TryGetMetadata_ReturnsFalse()
    {
        var response = new RequestResponse(RequestResponseStatus.OK_200);

        Assert.False(response.TryGetMetadata("key", out var value));
        Assert.Null(value);
    }

    [Fact]
    public void HasMetadataKey_ReturnsTrue()
    {
        var metadata = new Dictionary<string, object> { { "key", "value" } };
        var response = new RequestResponse(RequestResponseStatus.OK_200, metadata);
        
        Assert.True(response.HasMetadataKey("key"));
    }

    [Fact]
    public void HasMetadataKey_ReturnsFalse()
    {
        var response = new RequestResponse(RequestResponseStatus.OK_200);
        
        Assert.False(response.HasMetadataKey("key"));
    }

    [Fact]
    public void GetMetadataValue_ReturnsValue()
    {
        var metadata = new Dictionary<string, object> { { "key", "value" } };
        var response = new RequestResponse(RequestResponseStatus.OK_200, metadata);
        
        Assert.Equal("value", response.GetMetadataValue("key"));
    }

    [Fact]
    public void GetMetadataValue_ThrowsKeyNotFoundException()
    {
        var response = new RequestResponse(RequestResponseStatus.OK_200);

        Assert.Throws<KeyNotFoundException>(() => response.GetMetadataValue("key"));
    }
    
    [Fact]
    public void GetMetadataValueOrDefault_ReturnsValue()
    {
        var metadata = new Dictionary<string, object> { { "key", "value" } };
        var response = new RequestResponse(RequestResponseStatus.OK_200, metadata);
        
        Assert.Equal("value", response.GetMetadataValueOrDefault("key"));
    }

    [Fact]
    public void GetMetadataValueOrDefault_ReturnsNull()
    {
        var response = new RequestResponse(RequestResponseStatus.OK_200);
        
        Assert.Null(response.GetMetadataValueOrDefault("key"));
    }

    [Fact]
    public void GetMetadataValueOrDefault_ReturnsDefaultValue()
    {
        var response = new RequestResponse(RequestResponseStatus.OK_200);
        
        Assert.Equal("default", response.GetMetadataValueOrDefault("key", "default"));
    }
}