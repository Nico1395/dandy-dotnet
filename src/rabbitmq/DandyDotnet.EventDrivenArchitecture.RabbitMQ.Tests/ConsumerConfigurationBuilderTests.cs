using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

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

    private static ConsumerConfiguration Build(Action<ConsumerConfigurationBuilder> configure)
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(configure);
        return services.BuildServiceProvider().GetRequiredService<ConsumerConfiguration>();
    }
}
