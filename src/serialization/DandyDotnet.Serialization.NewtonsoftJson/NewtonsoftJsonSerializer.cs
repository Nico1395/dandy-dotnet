using DandyDotnet.Serialization.Abstractions;
using Newtonsoft.Json;

namespace DandyDotnet.Serialization.NewtonsoftJson;

internal sealed class NewtonsoftJsonSerializer(NewtonsoftJsonConfiguration configuration) : ISerializer
{
    public string Serialize(object item, Type? type)
    {
        type ??= item.GetType();
        return JsonConvert.SerializeObject(item, type, configuration.JsonSerializerSettings);
    }

    public object? Deserialize(string payload, Type type)
    {
        return JsonConvert.DeserializeObject(payload, type, configuration.JsonSerializerSettings);
    }
}