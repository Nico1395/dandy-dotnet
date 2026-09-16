namespace DandyDotnet.Patterns.EventSourcing.Configuration.Outbox;

/// <summary>
///     Represents configuration settings for the transactional outbox subsystem.
/// </summary>
public sealed class OutboxConfiguration
{
    /// <summary>
    ///     Gets the polling interval duration for the background outbox daemon.
    /// </summary>
    public TimeSpan Interval { get; internal set; } = TimeSpan.FromSeconds(3);

    /// <summary>
    ///     Gets the fallback lifetime duration for outbox events that do not specify their own lifetime.
    /// </summary>
    public TimeSpan DefaultEventLifetime { get; internal set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     Gets the default maximum number of delivery retries for outbox consumers.
    /// </summary>
    public int DefaultRetries { get; internal set; } = 3;

    /// <summary>
    ///     Gets a value indicating whether the hosted background outbox daemon is enabled.
    /// </summary>
    public bool DaemonEnabled { get; internal set; } = true;
}