namespace MonoCross.Utilities.Threading
{
    /// <summary>
    /// Provides methods for creating threading utilities.
    /// </summary>
    public static class ThreadFactory
    {
        /// <summary>
        /// Creates a new <see cref="IThread"/> instance.
        /// </summary>
        public static IThread Create()
        {
            IThread file = new BasicThread();
            return file;
        }

        /// <summary>
        /// Creates a new <see cref="IThread"/> instance of the specified thread type.
        /// </summary>
        /// <param name="threadType">The type of <see cref="IThread"/> to create.</param>
        public static IThread Create(ThreadType threadType)
        {
            IThread thread = new BasicThread();

            switch (threadType)
            {
                case ThreadType.MockThread:
                    thread = new MockThread();
                    break;
                default:
                    // returns the default - BasicThread implementation                 
                    break;
            }

            return thread;
        }
    }

    /// <summary>
    /// The available threading types.
    /// </summary>
    public enum ThreadType
    {
        /// <summary>
        /// Default thread type that works across multiple platforms.
        /// </summary>
        BasicThread,
        /// <summary>
        /// Mock thread type for platforms that don't support threading.
        /// </summary>
        MockThread
    }
}
