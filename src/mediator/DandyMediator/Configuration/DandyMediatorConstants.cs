namespace DandyMediator.Configuration;

/// <summary>
/// Constants used by DandyMediator.
/// </summary>
public static class DandyMediatorConstants
{
    /// <summary>
    /// Constants related to mediator plugins.
    /// </summary>
    public static class Plugins
    {
        /// <summary>
        /// Constants related to the validation plugin.
        /// </summary>
        public static class Validation
        {
            /// <summary>
            /// Validation plugin key.
            /// </summary>
            public const string Key = "mediator-validation";
            
            /// <summary>
            /// Validation plugin configuration slot.
            /// </summary>
            public const string Slot = "mediator-validation";
            
            /// <summary>
            /// Metadata key containing validation results.
            /// </summary>
            public const string RequestMetadataKey = "validation-result";
        }
    }
}
