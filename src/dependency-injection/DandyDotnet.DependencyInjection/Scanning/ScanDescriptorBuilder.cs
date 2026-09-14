using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ScanDescriptorBuilder(Type abstractType)
{
    private ServiceLifetime _lifetime = ServiceLifetime.Transient;
    private Func<Type, bool>? _predicate;
    private object? _key;
    private Func<IServiceProvider, object?, object>? _keyedFactory;
    private Func<IServiceProvider, object>? _factory;
    private bool _isOpenGeneric;

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

    public ScanDescriptorBuilder WithKey(object key)
    {
        _key = key;
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

    public ScanDescriptorBuilder AsOpenGeneric()
    {
        _isOpenGeneric = true;
        return this;
    }

    internal ScanDescriptor Build()
    {
        return new ScanDescriptor
        {
            AbstractType = abstractType,
            Lifetime = _lifetime,
            Predicate = _predicate,
            Key = _key,
            KeyedFactory = _keyedFactory,
            Factory = _factory,
            IsOpenGeneric = _isOpenGeneric,
        };
    }
}