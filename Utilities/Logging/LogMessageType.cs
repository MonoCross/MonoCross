namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// The available logger message levels.
    /// </summary>
    public enum LogMessageType
    {
        /// <summary>
        /// An informational message.
        /// </summary>
        Info = 3,
        /// <summary>
        /// A warning message.
        /// </summary>
        Warn = 4,
        /// <summary>
        /// A message for debugging.
        /// </summary>
        Debug = 1,
        /// <summary>
        /// A non-fatal error message.
        /// </summary>
        Error = 5,
        /// <summary>
        /// A fatal error message.
        /// </summary>
        Fatal = 6,
        /// <summary>
        /// A metric message.
        /// </summary>
        Metric = 2,
        /// <summary>
        /// A message about the current platform.
        /// </summary>
        Platform = 0,
    }
}
