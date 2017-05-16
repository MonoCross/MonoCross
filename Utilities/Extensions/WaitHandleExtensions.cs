namespace System.Threading
{
    /// <summary>
    /// Provides WaitHandle methods available in .NET 4.0
    /// </summary>
    public static class WaitHandleExtensions
    {
        /// <summary>
        /// Blocks the current thread until the current <see cref="WaitHandle"/> receives a signal, using a 32-bit signed integer to specify the time interval.
        /// </summary>
        /// <param name="handle">The current WaitHandle.</param>
        /// <param name="milliseconds">The number of milliseconds to wait, or <see cref="Timeout.Infinite"/> (-1) to wait indefinitely. </param>
        /// <returns><c>true</c> if the current instance receives a signal; otherwise, <c>false</c>.</returns>
        public static bool WaitOne(this WaitHandle handle, int milliseconds)
        {
            return handle.WaitOne(milliseconds, false);
        }
    }
}