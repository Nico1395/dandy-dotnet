using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class ConsumerResultTests
{
    [Theory]
    [InlineData(ConsumerStatus.Ack, false, false)]
    [InlineData(ConsumerStatus.Ack, false, true)]
    [InlineData(ConsumerStatus.Nack, false, false)]
    [InlineData(ConsumerStatus.Nack, true, false)]
    [InlineData(ConsumerStatus.Nack, false, true)]
    [InlineData(ConsumerStatus.Nack, true, true)]
    public void Factory_ReturnsExpectedResult(ConsumerStatus status, bool requeue, bool multiple)
    {
        var result = (status, requeue, multiple) switch
        {
            (ConsumerStatus.Ack, false, false) => ConsumerResult.Ack(),
            (ConsumerStatus.Ack, false, true) => ConsumerResult.AckMultiple(),
            (ConsumerStatus.Nack, false, false) => ConsumerResult.Nack(),
            (ConsumerStatus.Nack, true, false) => ConsumerResult.NackRequeue(),
            (ConsumerStatus.Nack, false, true) => ConsumerResult.NackMultiple(),
            _ => ConsumerResult.NackRequeueMultiple(),
        };

        Assert.Equal(status, result.Status);
        Assert.Equal(requeue, result.Requeue);
        Assert.Equal(multiple, result.Multiple);
    }
}
