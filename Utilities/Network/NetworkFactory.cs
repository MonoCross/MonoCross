namespace MonoCross.Utilities.Network
{
    /// <summary>
    /// Provides methods for creating a network access strategy.
    /// </summary>
    public static class NetworkFactory
    {
        /// <summary>
        /// Creates a new <see cref="INetwork"/> instance.
        /// </summary>
        public static INetwork Create()
        {
            INetwork network = new NetworkAsynch();
            return network;
        }

        // If we ever want to make an implementation of INetwork that we want in core,
        /// <summary>
        /// Creates a new <see cref="INetwork"/> instance of the specified network type.
        /// </summary>
        /// <param name="networkType">The type of <see cref="INetwork"/> to create.</param>
        /// <returns></returns>
        public static INetwork Create( NetworkType networkType )
        {
            INetwork network;

            switch (networkType)
            {
                case NetworkType.NetworkSynch:
                    network = new NetworkSynch();
                    break;
                case NetworkType.NetworkAsynch:
                default:
                    // returns the default - NetworkAsynch implementation
                    network = new NetworkAsynch();
                    break;
            }

            return network;
        }
    }

    /// <summary>
    /// The available networking types.
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        /// Default network type with asynchronous support for Get/Post methods.
        /// </summary>
        NetworkAsynch,
        /// <summary>
        /// Network type with synchronous support for Get/Post methods.
        /// </summary>
        NetworkSynch,
    }
}
