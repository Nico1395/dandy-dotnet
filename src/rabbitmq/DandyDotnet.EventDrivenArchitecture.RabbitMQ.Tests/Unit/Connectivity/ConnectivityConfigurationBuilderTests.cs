using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Connectivity;

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
        var node = Assert.Single(configuration.Nodes);
        Assert.Equal("localhost", node.HostName);
        Assert.Equal(5672, node.Port);
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

    [Fact]
    public void ConnectToCluster_ConfiguresMultipleEndpoints()
    {
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectToCluster("user", "password", [new Uri("amqp://one:5672"), new Uri("amqp://two:5673")], null)
            .Build();
        Assert.Equal(2, configuration.Nodes.Count);
        Assert.Collection(configuration.Nodes,
            node =>
            {
                Assert.Equal("one", node.HostName);
                Assert.Equal(5672, node.Port);
            },
            node =>
            {
                Assert.Equal("two", node.HostName);
                Assert.Equal(5673, node.Port);
            });
    }

    [Fact]
    public void ConnectionFactoryAction_CanConfigureRecovery()
    {
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectionFactory(factory => factory.AutomaticRecoveryEnabled = false)
            .Build();
        Assert.False(((global::RabbitMQ.Client.ConnectionFactory)configuration.ConnectionFactory).AutomaticRecoveryEnabled);
    }

    [Fact]
    public void ConnectToCluster_UsesDefaultRecoveryInterval()
    {
        var config = new ConnectivityConfigurationBuilder().ConnectToCluster("u", "p", [], null).Build();
        Assert.Equal(TimeSpan.FromSeconds(5), ((ConnectionFactory)config.ConnectionFactory).NetworkRecoveryInterval);
    }

    [Fact]
    public void SetConnectionFactory_UsesSuppliedFactory()
    {
        var factory = new ConnectionFactory { ClientProvidedName = "supplied" };
        var config = new ConnectivityConfigurationBuilder().SetConnectionFactory(factory).Build();
        Assert.Same(factory, config.ConnectionFactory);
    }
}