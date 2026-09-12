namespace DandyDotnet.Serialization.Abstractions;

public sealed class SerializerConfigurationBuilder
{
    private readonly SerializerConfiguration _configuration = new();

    public SerializerConfigurationBuilder UseSerializer(Type serializerType, object configuration)
    {
        _configuration.Type = serializerType;
        _configuration.Configuration = configuration;

        return this;
    }
    
    internal SerializerConfiguration Build()
    {
        return _configuration;
    }
}