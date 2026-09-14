using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.DependencyInjection.Tests.Abstractions.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Abstractions;

public sealed class KeyedServiceCollectionExtensionsTests
{
    private static readonly Type[] _implementationTypes =
    [
        typeof(FirstRangedService),
        typeof(SecondRangedService),
    ];

    [Fact]
    public void AddKeyedTransientOrDefault_Type_RegistersTransientImplementation()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedTransientOrDefault(
            typeof(IRangedService), "key", typeof(FirstRangedService));

        AssertDescriptor(services, result, ServiceLifetime.Transient, "key", typeof(FirstRangedService));
    }

    [Fact]
    public void AddKeyedTransientOrDefault_Generic_RegistersTransientImplementations()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedTransientOrDefault<IRangedService>("key", _implementationTypes);

        AssertDescriptors(services, result, ServiceLifetime.Transient, "key");
    }

    [Fact]
    public void AddKeyedTransientOrDefault_Type_RegistersTransientImplementations()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedTransientOrDefault(
            typeof(IRangedService), "key", _implementationTypes);

        AssertDescriptors(services, result, ServiceLifetime.Transient, "key");
    }

    [Fact]
    public void AddKeyedTransientOrDefault_Generic_RegistersTransientImplementation()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedTransientOrDefault<IRangedService>("key", typeof(FirstRangedService));

        AssertDescriptor(services, result, ServiceLifetime.Transient, "key", typeof(FirstRangedService));
    }

    [Fact]
    public void AddKeyedScopedOrDefault_Type_RegistersScopedImplementation()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedScopedOrDefault(
            typeof(IRangedService), "key", typeof(FirstRangedService));

        AssertDescriptor(services, result, ServiceLifetime.Scoped, "key", typeof(FirstRangedService));
    }

    [Fact]
    public void AddKeyedScopedOrDefault_Generic_RegistersScopedImplementations()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedScopedOrDefault<IRangedService>("key", _implementationTypes);

        AssertDescriptors(services, result, ServiceLifetime.Scoped, "key");
    }

    [Fact]
    public void AddKeyedScopedOrDefault_Type_RegistersScopedImplementations()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedScopedOrDefault(
            typeof(IRangedService), "key", _implementationTypes);

        AssertDescriptors(services, result, ServiceLifetime.Scoped, "key");
    }

    [Fact]
    public void AddKeyedScopedOrDefault_Generic_RegistersScopedImplementation()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedScopedOrDefault<IRangedService>("key", typeof(FirstRangedService));

        AssertDescriptor(services, result, ServiceLifetime.Scoped, "key", typeof(FirstRangedService));
    }

    [Fact]
    public void AddKeyedSingletonOrDefault_Type_RegistersSingletonImplementation()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedSingletonOrDefault(
            typeof(IRangedService), "key", typeof(FirstRangedService));

        AssertDescriptor(services, result, ServiceLifetime.Singleton, "key", typeof(FirstRangedService));
    }

    [Fact]
    public void AddKeyedSingletonOrDefault_Generic_RegistersSingletonImplementations()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedSingletonOrDefault<IRangedService>("key", _implementationTypes);

        AssertDescriptors(services, result, ServiceLifetime.Singleton, "key");
    }

    [Fact]
    public void AddKeyedSingletonOrDefault_Type_RegistersSingletonImplementations()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedSingletonOrDefault(
            typeof(IRangedService), "key", _implementationTypes);

        AssertDescriptors(services, result, ServiceLifetime.Singleton, "key");
    }

    [Fact]
    public void AddKeyedSingletonOrDefault_Generic_RegistersSingletonImplementation()
    {
        var services = new ServiceCollection();

        var result = services.AddKeyedSingletonOrDefault<IRangedService>("key", typeof(FirstRangedService));

        AssertDescriptor(services, result, ServiceLifetime.Singleton, "key", typeof(FirstRangedService));
    }

    [Fact]
    public void AddKeyedTransientOrDefault_AllowsNullServiceKey()
    {
        var services = new ServiceCollection();

        services.AddKeyedTransientOrDefault<IRangedService>(null, typeof(FirstRangedService));

        AssertDescriptor(services, services, ServiceLifetime.Transient, null, typeof(FirstRangedService));
    }

    private static void AssertDescriptor(
        IServiceCollection expected,
        IServiceCollection result,
        ServiceLifetime lifetime,
        object? key,
        Type implementationType)
    {
        Assert.Same(expected, result);

        var descriptor = Assert.Single(result);
        Assert.Equal(typeof(IRangedService), descriptor.ServiceType);
        Assert.Equal(lifetime, descriptor.Lifetime);
        Assert.Equal(implementationType, descriptor.IsKeyedService
            ? descriptor.KeyedImplementationType
            : descriptor.ImplementationType);
        Assert.Equal(key is not null, descriptor.IsKeyedService);
        Assert.Equal(key, descriptor.ServiceKey);
    }

    private static void AssertDescriptors(
        IServiceCollection expected,
        IServiceCollection result,
        ServiceLifetime lifetime,
        object key)
    {
        Assert.Same(expected, result);
        Assert.Equal(_implementationTypes.Length, result.Count);

        Assert.Equal(
            _implementationTypes,
            result.Select(descriptor => descriptor.KeyedImplementationType).ToArray());
        Assert.All(result, descriptor =>
        {
            Assert.Equal(typeof(IRangedService), descriptor.ServiceType);
            Assert.Equal(lifetime, descriptor.Lifetime);
            Assert.True(descriptor.IsKeyedService);
            Assert.Equal(key, descriptor.ServiceKey);
        });
    }
}
