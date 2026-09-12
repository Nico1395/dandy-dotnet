namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;

/// <summary>
/// Base class for attributes that configure message metadata.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public abstract class MessageAttribute : Attribute;
