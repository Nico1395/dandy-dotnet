using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Integration;

[CollectionDefinition(Name)]
public sealed class RabbitMqIntegrationCollection : ICollectionFixture<IntegrationFixture>
{
    public const string Name = "RabbitMQ integration";
}