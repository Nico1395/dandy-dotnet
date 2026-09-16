using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions.Interceptors;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Consumer;

public sealed class ConsumerConfigurationBuilderTests
{
    [Fact]
    public void ScanInAssemblies_StoresAssemblies()
    {
        var configuration = Build(x => x.ScanInAssemblies(typeof(ConsumerConfigurationBuilderTests).Assembly));

        Assert.Contains(typeof(ConsumerConfigurationBuilderTests).Assembly, configuration.Assemblies!);
    }

    [Fact]
    public void ExceptionHandlers_AreStored()
    {
        Action<IServiceProvider, Exception> initialize = (_, _) => { };
        Action<IServiceProvider, Exception> receive = (_, _) => { };
        Action<IServiceProvider, Exception> ack = (_, _) => { };
        Action<IServiceProvider, Exception> intercept = (_, _) => { };
        var configuration = Build(x => x
            .OnExceptionWhenInitializingWorker(initialize)
            .OnExceptionWhenReceivingMessage(receive)
            .OnExceptionWhenAckOrNack(ack)
            .OnExceptionWhenIntercepting(intercept));

        Assert.Same(initialize, configuration.OnExceptionWhenInitializingWorker);
        Assert.Same(receive, configuration.OnExceptionWhenReceivingMessage);
        Assert.Same(ack, configuration.OnExceptionWhenAckOrNack);
        Assert.Same(intercept, configuration.OnExceptionWhenIntercepting);
    }

    [Fact]
    public void UseConsumerInterceptor_StoresConfiguredType()
    {
        var configuration = Build(builder => builder.UseConsumerInterceptor(typeof(TestInterceptor)));

        Assert.Equal(typeof(TestInterceptor), configuration.ConsumerInterceptorType);
    }

    private sealed class TestInterceptor : IConsumerInterceptor
    {
        public Task OnAfterAckAsync(object message, ConsumerContext context,
            ConsumerResult result, CancellationToken cancellationToken) => Task.CompletedTask;

        public Task OnAfterNackAsync(object message, ConsumerContext context,
            ConsumerResult result, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private static ConsumerConfiguration Build(Action<ConsumerConfigurationBuilder> configure)
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(configure);
        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ConsumerConfiguration>();
    }
}