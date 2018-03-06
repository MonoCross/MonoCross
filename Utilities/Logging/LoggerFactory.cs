using MonoCross.Navigation;

namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// Represents a logging factory.
    /// </summary>
    internal static class LoggerFactory
    {
        /// <summary>
        /// Creates an ILog instance in the specified log path.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the Log path value.</param>
        /// <returns></returns>
        internal static ILog Create(string logPath)
        {
            return MXContainer.Resolve<ILog>((object)logPath);
        }

        /// <summary>
        /// Creates an ILog instance in the specified log path, and of the specified type.
        /// </summary>
        /// <param name="logPath">A <see cref="String"/> representing the Log path value.</param>
        /// <param name="loggerType">Type of the logger.</param>
        /// <returns></returns>
        internal static ILog Create(string logPath, LoggerType loggerType)
        {
            return MXContainer.Resolve<ILog>(loggerType.ToString(), (object)logPath);
        }
    }

    /// <summary>
    /// Represents the available logger types.
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// A Basic Logger.
        /// </summary>
        BasicLogger,
        //NLog,
    }
}
