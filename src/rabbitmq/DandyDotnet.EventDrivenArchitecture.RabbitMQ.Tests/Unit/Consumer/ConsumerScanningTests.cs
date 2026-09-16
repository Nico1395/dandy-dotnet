using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Consumer;

public sealed class ConsumerScanningTests
{
    [Fact]
    public void AssemblyScanning_RegistersConsumer()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.ScanInAssemblies(typeof(ScanConsumer).Assembly));
        using var provider = services.BuildServiceProvider();
        Assert.IsType<ScanConsumer>(provider.GetRequiredService<IConsumer<ScanMessage>>());
    }

    [Fact]
    public void AssemblyScanning_RegistersMiddleware()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.ScanInAssemblies(typeof(ScanMiddleware).Assembly));
        using var provider = services.BuildServiceProvider();
        Assert.IsType<ScanMiddleware>(Assert.Single(provider.GetServices<IConsumerMiddleware<ScanMessage>>()));
    }

    [Fact]
    public void AssemblyScanning_RegistersExceptionHandler()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.ScanInAssemblies(typeof(ScanExceptionHandler).Assembly));
        using var provider = services.BuildServiceProvider();
        Assert.IsType<ScanExceptionHandler>(provider.GetRequiredService<IConsumerExceptionHandler<ScanMessage>>());
    }

    [Fact]
    public void AssemblyScanning_WithNoAssemblies_DoesNotRegisterConsumers()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => { });
        using var provider = services.BuildServiceProvider();
        Assert.Null(provider.GetService<IConsumer<ScanMessage>>());
    }

    public sealed class ScanMessage
    {
    }

    public sealed class ScanConsumer : IConsumer<ScanMessage>
    {
        public Task<ConsumerResult> ConsumeAsync(ScanMessage message, ConsumerContext context, CancellationToken cancellationToken) => Task.FromResult(ConsumerResult.Ack());
    }

    public sealed class ScanMiddleware : IConsumerMiddleware<ScanMessage>
    {
        public Task<ConsumerResult> InterceptAsync(ScanMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken) => nextStep();
    }

    public sealed class ScanExceptionHandler : IConsumerExceptionHandler<ScanMessage>
    {
        public Task HandleAsync(ScanMessage message, ConsumerContext context, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}