using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class ConnectivityConfigurationBuilderTests
{
    [Fact]
    public void ConnectToCluster_ConfiguresCredentialsAndNodes()
    {
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectToCluster("user", "password", [new Uri("amqp://localhost:5672")], null)
            .Build();

        var factory = (ConnectionFactory)configuration.ConnectionFactory;
        Assert.Equal("user", factory.UserName);
        Assert.Equal("password", factory.Password);
        Assert.Single(configuration.Nodes);
        Assert.True(factory.AutomaticRecoveryEnabled);
        Assert.True(factory.TopologyRecoveryEnabled);
    }

    [Fact]
    public void ConnectToCluster_UsesConfiguredRecoveryInterval()
    {
        var interval = TimeSpan.FromSeconds(3);
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectToCluster("user", "password", [], interval)
            .Build();

        Assert.Equal(interval, ((ConnectionFactory)configuration.ConnectionFactory).NetworkRecoveryInterval);
    }

    [Fact]
    public void ConnectionFactoryAction_ConfiguresFactory()
    {
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectionFactory(factory => factory.ClientProvidedName = "tests")
            .Build();

        Assert.Equal("tests", configuration.ConnectionFactory.ClientProvidedName);
    }

    [Fact]
    public void OnConnectionException_StoresHandler()
    {
        Action<IServiceProvider, Exception> handler = (_, _) => { };
        var configuration = new ConnectivityConfigurationBuilder().OnConnectionException(handler).Build();

        Assert.Same(handler, configuration.OnConnectionException);
    }
}
