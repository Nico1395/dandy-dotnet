using DandyMediator.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DandyMediator.Validation;

/// <summary>
/// Contains extensions for <see cref="DandyMediatorConfigurationBuilder"/>.
/// </summary>
public static class DandyMediatorConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds validation to the mediator.
    /// </summary>
    /// <param name="builder">The mediator configuration builder to add validation to.</param>
    /// <param name="configuration">Configuration action to configure validation.</param>
    /// <returns>The mediator configuration builder.</returns>
    public static DandyMediatorConfigurationBuilder UseValidation(this DandyMediatorConfigurationBuilder builder, Action<DandyMediatorValidationPluginConfigurationBuilder>? configuration = null)
    {
        var configurationBuilder = new DandyMediatorValidationPluginConfigurationBuilder();
        configuration?.Invoke(configurationBuilder);
        var config = configurationBuilder.Build();

        var plugin = new DandyMediatorValidationPlugin
        {
            ConfigurationFactory = (services, _) =>
            {
                if (!config.Enabled)
                    return config;

                services.AddSingleton(config);
                services.AddTransient(typeof(IRequestMiddleware<,>), typeof(ResponseRequestValidationMiddleware<,>));
                services.AddSingleton<IRequestValidator, RequestValidator>();

                return config;
            },
        };

        return builder.UsePlugin(plugin);
    }
}
