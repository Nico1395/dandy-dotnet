namespace DandyRabbitMQ.Core.Declarations.Configuration;

/// <summary>
/// Builds RabbitMQ declaration configuration.
/// </summary>
public sealed class DeclarationsConfigurationBuilder
{
    private readonly DeclarationsConfiguration _configuration = new();
    /// <summary>
    /// Configures an <paramref name="exchange"/>, <paramref name="queue"/>, and channel subscription.
    /// </summary>
    /// <param name="exchange">The exchange name.</param>
    /// <param name="queue">The queue name.</param>
    /// <param name="channelAction">An action that configures the channel.</param>
    /// <returns>This builder.</returns>
    public DeclarationsConfigurationBuilder SubscribeChannel(string exchange, string queue, Action<ChannelConfiguration> channelAction)
    {
        var channel = new ChannelConfiguration(exchange, queue);
        channelAction(channel);

        _configuration.Channels[queue] = channel;
        return this;
    }

    /// <summary>
    /// Builds the declaration configuration.
    /// </summary>
    /// <returns>The declaration configuration.</returns>
    public DeclarationsConfiguration Build()
    {
        return _configuration;
    }
}
