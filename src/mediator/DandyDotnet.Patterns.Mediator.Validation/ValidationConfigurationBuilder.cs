namespace DandyDotnet.Patterns.Mediator.Validation;

/// <summary>
/// Builder for the validation plugin configuration.
/// </summary>
public sealed class ValidationConfigurationBuilder
{
    private readonly ValidationConfiguration _configuration = new();

    /// <summary>
    /// Enables or disables validation.
    /// </summary>
    /// <param name="enabled">Whether the plugin is enabled or disabled. <see langword="true"/> by default.</param>
    /// <returns>The builder.</returns>
    public ValidationConfigurationBuilder SetEnabled(bool enabled = true)
    {
        _configuration.Enabled = enabled;
        return this;
    }

    /// <summary>
    /// Sets the maximum recursion depth when validating requests with complex data structures.
    /// </summary>
    /// <param name="recursionDepth">Maximum recursion depth.</param>
    /// <returns>The builder.</returns>
    public ValidationConfigurationBuilder SetRecursionDepth(int recursionDepth)
    {
        _configuration.RecursionDepth = recursionDepth;
        return this;
    }

    internal ValidationConfiguration Build() => _configuration;
}
