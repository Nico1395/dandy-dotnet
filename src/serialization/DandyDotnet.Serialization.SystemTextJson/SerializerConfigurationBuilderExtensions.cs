using System.Text.Json;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.Serialization.SystemTextJson;

public static class SerializerConfigurationBuilderExtensions
{
    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder, SystemTextJsonConfiguration configuration)
    {
        return configurationBuilder.UseSerializer(typeof(SystemTextJsonSerializer), configuration);
    }

    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder, Action<SystemTextJsonConfiguration>? action)
    {
        var configuration = new SystemTextJsonConfiguration();
        action?.Invoke(configuration);
        return configurationBuilder.UseSystemTextJson(configuration);
    }

    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder.UseSystemTextJson(action: null);
    }

    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder, JsonSerializerOptions options)
    {
        return configurationBuilder.UseSystemTextJson(cfg => cfg.JsonSerializerOptions = options);
    }
}