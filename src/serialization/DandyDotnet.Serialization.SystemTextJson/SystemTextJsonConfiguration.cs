using System.Text.Json;

namespace DandyDotnet.Serialization.SystemTextJson;

public sealed class SystemTextJsonConfiguration
{
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
}