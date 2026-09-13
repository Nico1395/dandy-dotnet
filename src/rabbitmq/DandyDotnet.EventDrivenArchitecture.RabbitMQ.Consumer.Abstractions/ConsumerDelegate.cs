namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;

/// <summary>
/// Represents the next step in a consumer middleware pipeline.
/// </summary>
public delegate Task<ConsumerResult> ConsumerDelegate();
