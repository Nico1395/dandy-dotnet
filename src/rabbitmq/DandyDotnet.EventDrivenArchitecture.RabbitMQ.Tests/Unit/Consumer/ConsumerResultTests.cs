using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Consumer;

public sealed class ConsumerResultTests
{
    [Fact]
    public void Ack_ReturnsExpectedAcknowledgementFlags()
    {
        var result = ConsumerResult.Ack();

        Assert.Equal(ConsumerStatus.Ack, result.Status);
        Assert.False(result.Multiple);
        Assert.False(result.Requeue);
    }

    [Fact]
    public void AckMultiple_ReturnsExpectedAcknowledgementFlags()
    {
        var result = ConsumerResult.AckMultiple();

        Assert.Equal(ConsumerStatus.Ack, result.Status);
        Assert.True(result.Multiple);
        Assert.False(result.Requeue);
    }

    [Fact]
    public void Nack_ReturnsExpectedAcknowledgementFlags()
    {
        var result = ConsumerResult.Nack();

        Assert.Equal(ConsumerStatus.Nack, result.Status);
        Assert.False(result.Multiple);
        Assert.False(result.Requeue);
    }

    [Fact]
    public void NackRequeue_ReturnsExpectedAcknowledgementFlags()
    {
        var result = ConsumerResult.NackRequeue();

        Assert.Equal(ConsumerStatus.Nack, result.Status);
        Assert.False(result.Multiple);
        Assert.True(result.Requeue);
    }

    [Fact]
    public void NackMultiple_ReturnsExpectedAcknowledgementFlags()
    {
        var result = ConsumerResult.NackMultiple();

        Assert.Equal(ConsumerStatus.Nack, result.Status);
        Assert.True(result.Multiple);
        Assert.False(result.Requeue);
    }

    [Fact]
    public void NackRequeueMultiple_ReturnsExpectedAcknowledgementFlags()
    {
        var result = ConsumerResult.NackRequeueMultiple();

        Assert.Equal(ConsumerStatus.Nack, result.Status);
        Assert.True(result.Multiple);
        Assert.True(result.Requeue);
    }
}