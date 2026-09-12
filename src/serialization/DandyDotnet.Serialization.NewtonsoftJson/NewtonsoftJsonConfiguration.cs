using Newtonsoft.Json;

namespace DandyDotnet.Serialization.NewtonsoftJson;

public sealed class NewtonsoftJsonConfiguration
{
    public JsonSerializerSettings? JsonSerializerSettings { get; set; }
}