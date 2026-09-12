namespace DandyDotnet.Tests.Core.Mocks;

internal sealed class Item
{
    public required string Property { get; init; }

    public static Item New()
    {
        return new Item { Property = "Hello world!" };
    }

    public static Item New(string content)
    {
        return new Item { Property = content };
    }
}