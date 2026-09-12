namespace DandyDotnet.Serialization.Abstractions;

public static class SerializerExtensions
{
    public static string Serialize(this ISerializer serializer, object item)
    {
        return serializer.Serialize(item, item.GetType());
    }

    public static T? Deserialize<T>(this ISerializer serializer, string payload)
    {
        return serializer.Deserialize(payload, typeof(T)) is T casted ? casted : default;
    }
}