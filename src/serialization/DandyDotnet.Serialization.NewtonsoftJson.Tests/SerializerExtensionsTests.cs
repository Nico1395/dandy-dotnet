using DandyDotnet.Serialization.Abstractions;
using DandyDotnet.Serialization.NewtonsoftJson.Tests.Fixtures;
using DandyDotnet.Serialization.NewtonsoftJson.Tests.Mocks;

namespace DandyDotnet.Serialization.NewtonsoftJson.Tests;

public class SerializerExtensionsTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void Serialize_WithoutType_ReturnsJsonString()
    {
        var serializer = fixture.GetSerializer();
        var json = serializer.Serialize(Item.New());

        Assert.NotNull(json);
    }

    [Fact]
    public void Deserialize_WithGenericType_ReturnsObject()
    {
        var serializer = fixture.GetSerializer();
        var mock =  Item.New();
        var json = serializer.Serialize(mock);
        
        var deserialized = serializer.Deserialize<Item>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(mock.Property, deserialized.Property);
    }
}