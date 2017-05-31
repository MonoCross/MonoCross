namespace MonoCross.Utilities.Logging
{
    /// <summary>
    /// The level of verbosity when logging messages.
    /// </summary>
    public enum LoggingLevel
    {
        /// <summary>
        ///Logging level 1.  Platform messages and above are logged.  This is the most verbose logging level.
        /// </summary>
        Platform = 0,
        /// <summary>
        ///Logging level 2.  Debug messages and above are logged.
        /// </summary>
        Debug = 1,
        /// <summary>
        ///Logging level 3.  Metric messages and above are logged.
        /// </summary>
        Metric = 2,
        /// <summary>
        ///Logging level 4.  Info messages and above are logged.  
        /// </summary>
        Info = 3,
        /// <summary>
        ///Logging level 5.  Warning messages and above are logged.
        /// </summary>
        Warn = 4,
        /// <summary>
        ///Logging level 6.  Error messages and above are logged.
        /// </summary>
        Error = 5,
        /// <summary>
        ///Logging level 7.  Only fatal messages are logged.  This is the least verbose logging level.
        /// </summary>
        Fatal = 6,
    }
}