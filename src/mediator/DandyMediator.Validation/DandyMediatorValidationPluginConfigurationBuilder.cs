namespace DandyMediator.Validation;

/// <summary>
/// Builder for the validation plugin configuration.
/// </summary>
public sealed class DandyMediatorValidationPluginConfigurationBuilder
{
    private readonly DandyMediatorValidationPluginConfiguration _configuration = new();

    /// <summary>
    /// Enables or disables validation.
    /// </summary>
    /// <param name="enabled">Whether the plugin is enabled or disabled. <see langword="true"/> by default.</param>
    /// <returns>The builder.</returns>
    public DandyMediatorValidationPluginConfigurationBuilder SetEnabled(bool enabled = true)
    {
        _configuration.Enabled = enabled;
        return this;
    }

    /// <summary>
    /// Sets the maximum recursion depth when validating requests with complex data structures.
    /// </summary>
    /// <param name="recursionDepth">Maximum recursion depth.</param>
    /// <returns>The builder.</returns>
    public DandyMediatorValidationPluginConfigurationBuilder SetRecursionDepth(int recursionDepth)
    {
        _configuration.RecursionDepth = recursionDepth;
        return this;
    }

    internal DandyMediatorValidationPluginConfiguration Build() => _configuration;
}
