namespace DandyRabbitMQ.Core.Declarations.Configuration;

/// <summary>
/// Configures a RabbitMQ exchange declaration.
/// </summary>
public sealed class ExchangeConfiguration(string name)
{
    /// <summary>
    /// Gets the exchange name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets or sets the exchange type.
    /// </summary>
    public string ExchangeType { get; set; } = RabbitMQ.Client.ExchangeType.Topic;

    /// <summary>
    /// Gets or sets a value indicating whether the exchange is durable.
    /// </summary>
    public bool Durable { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the exchange is deleted automatically.
    /// </summary>
    public bool AutoDelete { get; set; } = false;
}
