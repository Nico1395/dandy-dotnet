using DandyRabbitMQ.Core.Messages.Configuration;
using DandyRabbitMQ.Producer.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Producer.Tests.Fixtures;

public sealed class DefaultFixture : IServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    
    public DefaultFixture()
    {
        var services = new ServiceCollection();

        services.AddDandyRabbitMQMessages(config =>
        {
            config.AddMessage(typeof(ConfiguredMessage), msg =>
            {
                msg.SetExchange("exchange");
                msg.SetRoutingKeys("routing-key");
            });
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
    
    public MessagesConfiguration GetMessagesConfiguration()
    {
        return _serviceProvider.GetRequiredService<MessagesConfiguration>();
    }
}