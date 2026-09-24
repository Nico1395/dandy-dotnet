using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Producer;

public sealed class ProducerTests
{
    [Fact]
    public async Task ProduceAsync_SerializesMessageAndPublishesOncePerNormalizedKey()
    {
        var (channel, recording) = RecordingChannel.Create();
        var (connection, _) = RecordingConnection.Create(channel);
        using var provider = CreateServices(connection);
        using var scope = provider.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        var properties = new BasicProperties { CorrelationId = "correlation" };
        var message = new TestMessage("hello");
        using var cancellation = new CancellationTokenSource();

        await producer.ProduceAsync("exchange", [" one ", "one", "", "two"], message, properties, cancellation.Token);

        Assert.Equal(["one", "two"], recording.Calls.Select(call => call.Arguments["routingKey"]));
        Assert.All(recording.Calls, call =>
        {
            Assert.Equal(nameof(IChannel.BasicPublishAsync), call.Method);
            Assert.Equal("exchange", call.Arguments["exchange"]);
            Assert.Equal(true, call.Arguments["mandatory"]);
            Assert.Same(properties, call.Arguments["basicProperties"]);
            Assert.Equal(nameof(TestMessage), properties.Type);
            Assert.Equal(cancellation.Token, call.Arguments["cancellationToken"]);
            var body = Assert.IsType<ReadOnlyMemory<byte>>(call.Arguments["body"]);
            Assert.Equal(message, System.Text.Json.JsonSerializer.Deserialize<TestMessage>(body.Span));
        });
    }

    [Fact]
    public async Task ProduceAsync_WithRepeatedCalls_ReusesCreatedChannel()
    {
        var (channel, recording) = RecordingChannel.Create();
        var (connection, connectionRecording) = RecordingConnection.Create(channel);
        using var provider = CreateServices(connection);
        using var scope = provider.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        using var cancellation = new CancellationTokenSource();

        await producer.ProduceAsync("exchange", ["one"], new TestMessage("first"), null, cancellation.Token);
        await producer.ProduceAsync("exchange", ["two"], new TestMessage("second"), null, cancellation.Token);

        Assert.Equal(cancellation.Token, Assert.Single(connectionRecording.ChannelCreationTokens));
        Assert.Equal(["one", "two"], recording.Calls.Select(call => call.Arguments["routingKey"]));
    }

    [Fact]
    public async Task ProduceAsync_WhenPublishFails_PropagatesExceptionAndCreatesNewChannelOnNextCall()
    {
        var (failedChannel, failedRecording) = RecordingChannel.Create();
        var (nextChannel, nextRecording) = RecordingChannel.Create();
        var exception = new InvalidOperationException("publish failed");
        failedRecording.PublishException = exception;
        var (connection, connectionRecording) = RecordingConnection.Create(failedChannel, nextChannel);
        using var provider = CreateServices(connection);
        using var scope = provider.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            producer.ProduceAsync("exchange", ["one", "two"], new TestMessage("first"), null, CancellationToken.None));
        await producer.ProduceAsync("exchange", ["retry"], new TestMessage("second"), null, CancellationToken.None);

        Assert.Same(exception, thrown);
        Assert.Single(failedRecording.Calls);
        Assert.Equal("retry", Assert.Single(nextRecording.Calls).Arguments["routingKey"]);
        Assert.Equal(2, connectionRecording.ChannelCreationTokens.Count);
    }

    private static ServiceProvider CreateServices(IConnectionProvider connection)
    {
        var services = new ServiceCollection();
        services.AddSingleton(connection);
        services.AddEncoder();
        services.AddSerialization(builder => builder.UseSystemTextJson());
        services.AddRabbitMQProducer(_ => { });
        return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
    }

    public sealed record TestMessage(string Content);
}