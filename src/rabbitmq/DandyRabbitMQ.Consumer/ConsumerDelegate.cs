namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Represents the next step in a consumer middleware pipeline.
/// </summary>
public delegate Task<ConsumerResult> ConsumerDelegate();
