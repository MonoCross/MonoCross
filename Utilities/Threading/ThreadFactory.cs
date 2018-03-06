using MonoCross.Navigation;

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
            return MXContainer.Resolve<IThread>();
        }

        /// <summary>
        /// Creates a new <see cref="IThread"/> instance of the specified thread type.
        /// </summary>
        /// <param name="threadType">The type of <see cref="IThread"/> to create.</param>
        public static IThread Create(ThreadType threadType)
        {
            return MXContainer.Resolve<IThread>(threadType.ToString());
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
