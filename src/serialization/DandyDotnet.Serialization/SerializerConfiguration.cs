using DandyDotnet.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization;

/// <summary>
/// Base configuration of the serializer implementation.
/// </summary>
public abstract class SerializerConfiguration
{
    /// <summary>
    /// Allows configuring services for the serializer implementation.
    /// </summary>
    /// <param name="configuration">The serialization configuration to add the serializer to.</param>
    /// <param name="services">The service collection to register services to.</param>
    protected internal virtual void ConfigureServices(SerializationConfiguration configuration, IServiceCollection services)
    {
        services.AddKeyedSingletonOrDefault(GetType(), configuration.ServiceKey, this);
    }
}