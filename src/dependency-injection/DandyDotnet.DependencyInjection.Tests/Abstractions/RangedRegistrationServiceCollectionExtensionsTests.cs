using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.DependencyInjection.Tests.Abstractions.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Abstractions;

public sealed class RangedRegistrationServiceCollectionExtensionsTests
{
   private static readonly Type[] _implementationTypes =
   [
       typeof(FirstRangedService),
       typeof(SecondRangedService),
   ];

   [Fact]
   public void AddRange_Type_RegistersAllImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddRange(typeof(IRangedService), _implementationTypes, ServiceLifetime.Scoped);

       AssertDescriptors(services, result, ServiceLifetime.Scoped);
   }

   [Fact]
   public void AddRange_Generic_RegistersAllImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddRange<IRangedService>(_implementationTypes, ServiceLifetime.Singleton);

       AssertDescriptors(services, result, ServiceLifetime.Singleton);
   }

   [Fact]
   public void AddKeyedRange_Type_RegistersAllImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedRange(typeof(IRangedService), "key", _implementationTypes, ServiceLifetime.Scoped);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Scoped, "key");
   }

   [Fact]
   public void AddKeyedRange_Generic_RegistersAllImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedRange<IRangedService>("key", _implementationTypes, ServiceLifetime.Singleton);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Singleton, "key");
   }

   [Fact]
   public void AddTransientRange_Type_RegistersTransientImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddTransientRange(typeof(IRangedService), _implementationTypes);

       AssertDescriptors(services, result, ServiceLifetime.Transient);
   }

   [Fact]
   public void AddTransientRange_Generic_RegistersTransientImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddTransientRange<IRangedService>(_implementationTypes);

       AssertDescriptors(services, result, ServiceLifetime.Transient);
   }

   [Fact]
   public void AddKeyedTransientRange_Type_RegistersTransientImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedTransientRange(typeof(IRangedService), "key", _implementationTypes);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Transient, "key");
   }

   [Fact]
   public void AddKeyedTransientRange_Generic_RegistersTransientImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedTransientRange<IRangedService>("key", _implementationTypes);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Transient, "key");
   }

   [Fact]
   public void AddScopedRange_Type_RegistersScopedImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddScopedRange(typeof(IRangedService), _implementationTypes);

       AssertDescriptors(services, result, ServiceLifetime.Scoped);
   }

   [Fact]
   public void AddScopedRange_Generic_RegistersScopedImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddScopedRange<IRangedService>(_implementationTypes);

       AssertDescriptors(services, result, ServiceLifetime.Scoped);
   }

   [Fact]
   public void AddKeyedScopedRange_Type_RegistersScopedImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedScopedRange(typeof(IRangedService), "key", _implementationTypes);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Scoped, "key");
   }

   [Fact]
   public void AddKeyedScopedRange_Generic_RegistersScopedImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedScoped<IRangedService>("key", _implementationTypes);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Scoped, "key");
   }

   [Fact]
   public void AddSingletonRange_Type_RegistersSingletonImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddSingletonRange(typeof(IRangedService), _implementationTypes);

       AssertDescriptors(services, result, ServiceLifetime.Singleton);
   }

   [Fact]
   public void AddSingletonRange_Generic_RegistersSingletonImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddSingletonRange<IRangedService>(_implementationTypes);

       AssertDescriptors(services, result, ServiceLifetime.Singleton);
   }

   [Fact]
   public void AddKeyedSingletonRange_Type_RegistersSingletonImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedSingletonRange(typeof(IRangedService), "key", _implementationTypes);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Singleton, "key");
   }

   [Fact]
   public void AddKeyedSingletonRange_Generic_RegistersSingletonImplementations()
   {
       var services = new ServiceCollection();

       var result = services.AddKeyedSingletonRange<IRangedService>("key", _implementationTypes);

       AssertKeyedDescriptors(services, result, ServiceLifetime.Singleton, "key");
   }

   [Fact]
   public void AddKeyedRange_AllowsNullServiceKey()
   {
       var services = new ServiceCollection();

       services.AddKeyedRange<IRangedService>(null, [typeof(FirstRangedService)], ServiceLifetime.Transient);

       var descriptor = Assert.Single(services);
       Assert.False(descriptor.IsKeyedService);
       Assert.Equal(typeof(FirstRangedService), descriptor.ImplementationType);
   }

   private static void AssertDescriptors(
       IServiceCollection expected,
       IServiceCollection result,
       ServiceLifetime lifetime)
   {
       Assert.Same(expected, result);
       Assert.Equal(_implementationTypes.Length, result.Count);

       Assert.Equal(
           _implementationTypes,
           result.Select(descriptor => descriptor.ImplementationType).ToArray());
       Assert.All(result, descriptor =>
       {
           Assert.Equal(typeof(IRangedService), descriptor.ServiceType);
           Assert.Equal(lifetime, descriptor.Lifetime);
           Assert.False(descriptor.IsKeyedService);
       });
   }

   private static void AssertKeyedDescriptors(
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