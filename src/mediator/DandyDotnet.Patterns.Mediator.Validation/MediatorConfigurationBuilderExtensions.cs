using DandyDotnet.Patterns.Mediator.Configuration;

namespace DandyDotnet.Patterns.Mediator.Validation;

/// <summary>
/// Contains extensions for <see cref="MediatorConfigurationBuilder"/>.
/// </summary>
public static class MediatorConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds validation to the mediator.
    /// </summary>
    /// <param name="builder">The mediator action builder to add validation to.</param>
    /// <param name="action">Configuration action to configure validation.</param>
    /// <returns>The mediator action builder.</returns>
    public static MediatorConfigurationBuilder UseValidation(this MediatorConfigurationBuilder builder, Action<ValidationConfigurationBuilder>? action = null)
    {
        var configurationBuilder = new ValidationConfigurationBuilder();
        action?.Invoke(configurationBuilder);
        var configuration = configurationBuilder.Build();

        return builder.UsePlugin(configuration);
    }
}
