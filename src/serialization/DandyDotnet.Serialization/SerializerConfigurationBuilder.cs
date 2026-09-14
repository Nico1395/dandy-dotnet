namespace DandyDotnet.Serialization;

public sealed class SerializerConfigurationBuilder
{
    private readonly SerializerConfiguration _configuration = new();

    public SerializerConfigurationBuilder UseServiceKey(object? serviceKey)
    {
        _configuration.ServiceKey = serviceKey;
        return this;
    }

    public SerializerConfigurationBuilder UseSerializer(Type serializerType, object configuration)
    {
        _configuration.SerializerType = serializerType;
        _configuration.Configuration = configuration;

        return this;
    }
    
    public SerializerConfiguration Build()
    {
        return _configuration;
    }
}