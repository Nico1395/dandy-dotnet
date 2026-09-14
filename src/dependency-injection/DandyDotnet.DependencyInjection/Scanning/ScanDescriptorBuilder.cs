using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ScanDescriptorBuilder(Type abstractType)
{
    private ServiceLifetime _lifetime = ServiceLifetime.Transient;
    private Func<Type, bool>? _predicate;
    private object? _serviceKey;
    private Func<Type, object?>? _serviceKeyFactory;
    private Func<IServiceProvider, object?, object>? _keyedFactory;
    private Func<IServiceProvider, object>? _factory;
    private bool _allowOpenGeneric;

    public ScanDescriptorBuilder When(Func<Type, bool> predicate)
    {
        _predicate = predicate;
        return this;
    }

    public ScanDescriptorBuilder WithLifetime(ServiceLifetime lifetime)
    {
        _lifetime = lifetime;
        return this;
    }

    public ScanDescriptorBuilder WithKey(object? serviceKey)
    {
        _serviceKey = serviceKey;
        return this;
    }

    public ScanDescriptorBuilder WithKey(Func<Type, object?> keyFactory)
    {
        _serviceKeyFactory = keyFactory;
        return this;
    }

    public ScanDescriptorBuilder WithKeyedFactory(Func<IServiceProvider, object?, object> keyedFactory)
    {
        _keyedFactory = keyedFactory;
        return this;
    }

    public ScanDescriptorBuilder WithFactory(Func<IServiceProvider, object> factory)
    {
        _factory = factory;
        return this;
    }

    public ScanDescriptorBuilder AllowOpenGeneric()
    {
        _allowOpenGeneric = true;
        return this;
    }

    internal ScanDescriptor Build()
    {
        return new ScanDescriptor
        {
            AbstractType = abstractType,
            Lifetime = _lifetime,
            Predicate = _predicate,
            ServiceKey = _serviceKey,
            ServiceKeyFactory = _serviceKeyFactory,
            KeyedFactory = _keyedFactory,
            Factory = _factory,
            AllowOpenGeneric = _allowOpenGeneric,
        };
    }
}