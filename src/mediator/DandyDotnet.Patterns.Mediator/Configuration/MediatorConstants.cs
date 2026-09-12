namespace DandyDotnet.Patterns.Mediator.Configuration;

/// <summary>
/// Constants used by DandyDotnet.Patterns.Mediator.
/// </summary>
public static class MediatorConstants
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
            /// Validation plugin configuration slot.
            /// </summary>
            public const string Slot = "validation";

            /// <summary>
            /// Metadata key containing validation results.
            /// </summary>
            public const string RequestMetadataKey = "validation-result";
        }
        
        /// <summary>
        /// Constants related to the queries plugin.
        /// </summary>
        public static class Queries
        {
            /// <summary>
            /// Queries plugin configuration slot.
            /// </summary>
            public const string Slot = "queries";
        }
        
        /// <summary>
        /// Constants related to the commands plugin.
        /// </summary>
        public static class Commands
        {
            /// <summary>
            /// Commands plugin configuration slot.
            /// </summary>
            public const string Slot = "commands";
        }
    }
}
