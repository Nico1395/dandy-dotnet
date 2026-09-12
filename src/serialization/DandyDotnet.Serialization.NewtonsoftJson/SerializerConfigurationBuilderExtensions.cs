using DandyDotnet.Serialization.Abstractions;
using Newtonsoft.Json;

namespace DandyDotnet.Serialization.NewtonsoftJson;

public static class SerializerConfigurationBuilderExtensions
{
    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder, NewtonsoftJsonConfiguration configuration)
    {
        return configurationBuilder.UseSerializer(typeof(NewtonsoftJsonSerializer), configuration);
    }

    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder, Action<NewtonsoftJsonConfiguration>? action)
    {
        var configuration = new NewtonsoftJsonConfiguration();
        action?.Invoke(configuration);
        return configurationBuilder.UseNewtonsoftJson(configuration);
    }

    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder.UseNewtonsoftJson(action: null);
    }
    
    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder, JsonSerializerSettings settings)
    {
        return configurationBuilder.UseNewtonsoftJson(cfg => cfg.JsonSerializerSettings = settings);
    }
}