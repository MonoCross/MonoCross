using MonoCross.Utilities;

namespace MonoCross.Utilities.Compatibility
{
    /// <summary>
    /// Static helper class for working with directories.
    /// </summary>
    public static class Directory
    {
        /// <summary>
        /// Returns a value indicating whether or not the specified directory exists.
        /// </summary>
        /// <param name="directoryName">The directory to check for.</param>
        /// <returns><c>true</c> if the specified directory exists; otherwise <c>false</c>.</returns>
        public static bool Exists(string directoryName)
        {
            return Device.File.Exists(directoryName);
        }

        /// <summary>
        /// Creates a directory with the specified name.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        public static void CreateDirectory(string directoryName)
        {
            Device.File.CreateDirectory(directoryName);
        }

        /// <summary>
        /// Returns the names of the files that are inside of the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the files of.</param>
        public static string[] GetFiles(string directoryName)
        {
            return Device.File.GetFileNames(directoryName);
        }

        /// <summary>
        /// Returns the names of subdirectories within the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the subdirectories of.</param>
        public static string[] GetDirectories(string directoryName)
        {
            return Device.File.GetDirectoryNames(directoryName);
        }
    }
}
