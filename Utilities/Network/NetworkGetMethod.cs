namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// The available restrictions that can be imposed on network GET methods.
    /// </summary>
    public enum NetworkGetMethod
    {
        /// <summary>
        /// No restrictions.
        /// </summary>
        Any,
        /// <summary>
        /// Caching is not allowed.
        /// </summary>
        NoCache
    }
}
