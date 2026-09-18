namespace DandyDotnet.Patterns.EventSourcing.Configuration.Projections;

public sealed class ProjectionsConfigurationBuilder
{
    private readonly ProjectionsConfiguration _configuration = new();

    public ProjectionsConfigurationBuilder AddProjection<TProjection>(Action<ProjectionConfigurationBuilder<TProjection>> builderAction)
        where TProjection : class
    {
        var builder = new ProjectionConfigurationBuilder<TProjection>();
        builderAction(builder);
        var configuration = builder.Build();

        _configuration.ProjectionConfigsByType[configuration.RuntimeType] = configuration;
        _configuration.ProjectionConfigsByKey[configuration.Key] = configuration;

        return this;
    }
    
    internal ProjectionsConfiguration Build()
    {
        return _configuration;
    }
}