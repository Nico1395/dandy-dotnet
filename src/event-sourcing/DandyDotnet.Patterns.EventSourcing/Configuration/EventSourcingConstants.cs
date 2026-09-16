namespace DandyDotnet.Patterns.EventSourcing.Configuration;

/// <summary>
///     Provides common constant values used throughout the Event Sourcing library.
/// </summary>
public static class EventSourcingConstants
{
    /// <summary>
    ///     The default service key used for keyed dependency injection registrations in Event Sourcing.
    /// </summary>
    public const string ServiceKey = "event-store";
}