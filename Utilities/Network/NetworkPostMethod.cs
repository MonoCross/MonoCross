namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// The available restrictions that can be imposed on network POST methods.
    /// </summary>
    public enum NetworkPostMethod
    {
        /// <summary>
        /// No restrictions.
        /// </summary>
        Any,
        /// <summary>
        /// Synchronous methods are allowed.
        /// </summary>
        ImmediateSynchronous,
        /// <summary>
        /// Asynchronous methods are allowed.
        /// </summary>
        QueuedAsynchronous,
    }
}
