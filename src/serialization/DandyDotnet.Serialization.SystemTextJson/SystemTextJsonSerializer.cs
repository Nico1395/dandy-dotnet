using System.Text.Json;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.Serialization.SystemTextJson;

internal sealed class SystemTextJsonSerializer(SystemTextJsonConfiguration configuration) : ISerializer
{
    public string Serialize(object item, Type? type)
    {
        type ??= item.GetType();
        return JsonSerializer.Serialize(item, type, configuration.JsonSerializerOptions);
    }

    public object? Deserialize(string payload, Type type)
    {
        return JsonSerializer.Deserialize(payload, type, configuration.JsonSerializerOptions);
    }
}