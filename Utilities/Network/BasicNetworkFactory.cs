namespace MonoCross.Utilities.Networking
{
    /// <summary>
    /// Provides methods for creating a network access strategy.
    /// </summary>
    public static class BasicNetworkFactory
    {
        /// <summary>
        /// Creates a new <see cref="INetwork"/> instance.
        /// </summary>
        public static INetwork Create()
        {
            INetwork network = new BasicNetworkAsynch();
            return network;
        }

        // If we ever want to make an implementation of INetwork that we want in core,
        /// <summary>
        /// Creates a new <see cref="INetwork"/> instance of the specified network type.
        /// </summary>
        /// <param name="networkType">The type of <see cref="INetwork"/> to create.</param>
        /// <returns></returns>
        public static INetwork Create(NetworkType networkType)
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
                    network = new BasicNetworkAsynch();
                    break;
            }

            return network;
        }
    }
}
