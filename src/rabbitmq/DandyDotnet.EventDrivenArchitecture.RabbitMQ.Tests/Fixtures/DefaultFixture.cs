using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddEncoder();
        services.AddRabbitMQMessages(config =>
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