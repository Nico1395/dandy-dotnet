namespace DandyDotnet.Serialization.Abstractions;

public interface ISerializer
{
    string Serialize(object item, Type? type);
    object? Deserialize(string payload, Type type);
}