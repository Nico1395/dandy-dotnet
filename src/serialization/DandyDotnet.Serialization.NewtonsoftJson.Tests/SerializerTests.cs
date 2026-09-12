using DandyDotnet.Serialization.NewtonsoftJson.Tests.Fixtures;
using DandyDotnet.Serialization.NewtonsoftJson.Tests.Mocks;

namespace DandyDotnet.Serialization.NewtonsoftJson.Tests;

public class SerializerTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void Serialize_ReturnsJsonString()
    {
        var serializer = fixture.GetSerializer();
        var json = serializer.Serialize(Item.New(), typeof(Item));

        Assert.NotNull(json);
    }

    [Fact]
    public void Deserialize_ReturnsObject()
    {
        var serializer = fixture.GetSerializer();
        var mock = Item.New();
        var json = serializer.Serialize(mock, typeof(Item));

        var deserialized = serializer.Deserialize(json, typeof(Item));

        Assert.NotNull(deserialized);
        Assert.IsType<Item>(deserialized);
        Assert.Equal(mock.Property, ((Item)deserialized).Property);
    }
}