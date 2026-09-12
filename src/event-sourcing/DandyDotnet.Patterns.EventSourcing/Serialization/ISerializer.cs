namespace DandyDotnet.Patterns.EventSourcing.Serialization;

public interface ISerializer
{
    string Serialize(object item, Type runtimeType);
    object? Deserialize(string payload, Type eventType);
}