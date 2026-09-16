namespace DandyDotnet.Patterns.EventSourcing.Configuration.Outbox;

/// <summary>
///     Provides a fluent builder for configuring the transactional outbox subsystem and background processing daemon.
/// </summary>
public sealed class OutboxConfigurationBuilder
{
    private readonly OutboxConfiguration _configuration = new();

    /// <summary>
    ///     Sets the execution interval for the background outbox daemon poll cycle.
    /// </summary>
    /// <param name="interval">The polling interval duration.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public OutboxConfigurationBuilder WithInterval(TimeSpan interval)
    {
        _configuration.Interval = interval;
        return this;
    }

    /// <summary>
    ///     Sets the default lifetime for outbox events before they expire.
    /// </summary>
    /// <param name="lifetime">The default expiration duration.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public OutboxConfigurationBuilder WithDefaultEventLifetime(TimeSpan lifetime)
    {
        _configuration.DefaultEventLifetime = lifetime;
        return this;
    }

    /// <summary>
    ///     Sets the default maximum number of delivery retry attempts for outbox event consumers.
    /// </summary>
    /// <param name="retries">The maximum retry count.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public OutboxConfigurationBuilder WithDefaultRetries(int retries)
    {
        _configuration.DefaultRetries = retries;
        return this;
    }

    /// <summary>
    ///     Disables the hosted background outbox daemon from automatically starting.
    /// </summary>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public OutboxConfigurationBuilder DisableDaemon()
    {
        _configuration.DaemonEnabled = false;
        return this;
    }

    internal OutboxConfiguration Build()
    {
        return _configuration;
    }
}