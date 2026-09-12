using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Mediator.Validation;

internal sealed class ValidationConfiguration : MediatorPluginConfiguration
{
    public override string Slot => MediatorConstants.Plugins.Validation.Slot;

    public bool Enabled { get; internal set; } = true;
    public int RecursionDepth { get; internal set; } = 10;
    
    public override void ConfigureServices(IServiceCollection services)
    {
        if (!Enabled)
            return;

        services.AddSingleton(this);
        services.AddTransient(typeof(IRequestMiddleware<,>), typeof(ResponseRequestValidationMiddleware<,>));
        services.AddSingleton<IRequestValidator, RequestValidator>();
    }
}
