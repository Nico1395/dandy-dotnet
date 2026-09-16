namespace DandyDotnet.Persistence.Sql.Migrations;

/// <summary>
///     Contains constants used by the migrations framework.
/// </summary>
public static class MigrationsConstants
{
    /// <summary>
    ///     Contains constants related to database tables used by the migrations framework.
    /// </summary>
    public static class Tables
    {
        /// <summary>
        ///     Contains constants related to the migrations tracking table.
        /// </summary>
        public static class Migrations
        {
            /// <summary>
            ///     The default name of the migrations tracking table.
            /// </summary>
            /// <returns>The table name as a string. Default is "__migrations".</returns>
            /// <remarks>
            ///     <para>
            ///         This table stores information about applied migrations, including their version numbers
            ///         and the timestamp when they were applied.
            ///     </para>
            ///     <para>
            ///         The table name can be customized through the <see cref="MigrationsConfiguration.Table" /> property.
            ///     </para>
            /// </remarks>
            public const string TableName = "__migrations";

            /// <summary>
            ///     The name of the column that stores the migration version number.
            /// </summary>
            /// <returns>The column name as a string. Default is "version".</returns>
            public const string Version = "version";

            /// <summary>
            ///     The name of the column that stores the timestamp when the migration was applied.
            /// </summary>
            /// <returns>The column name as a string. Default is "applied_at".</returns>
            public const string AppliedAt = "applied_at";
        }
    }
}