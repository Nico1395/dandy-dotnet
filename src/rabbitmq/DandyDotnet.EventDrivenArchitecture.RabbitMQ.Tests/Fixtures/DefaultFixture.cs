using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDandyRabbitMQMessages(config =>
        {
            config.AddMessage(typeof(ConfiguredMessage), msg =>
            {
                msg.SetExchange("exchange");
                msg.SetRoutingKeys("routing-key");
            });
        });
    }

    public MessagesConfiguration GetMessagesConfiguration()
    {
        return ServiceProvider.GetRequiredService<MessagesConfiguration>();
    }
}