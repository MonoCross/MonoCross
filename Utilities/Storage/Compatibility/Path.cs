using MonoCross;
using MonoCross.Utilities;

namespace MonoCross.Utilities.Compatibility
{
    /// <summary>
    /// Static helper class for working with paths.
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// Combines the specified strings into a path.
        /// </summary>
        /// <param name="paths">The parts of the path to combine.</param>
        /// <returns>The fully combined path.</returns>
        public static string Combine(params string[] paths)
        {
            if (paths.Length == 0) return null;
            var retval = paths[0];
            for (var i = 1; i < paths.Length; i++)
                retval = retval.AppendPath(paths[i]);
            return retval;
        }

        /// <summary>
        /// Returns the name of the directory that contains the specified file.
        /// </summary>
        /// <param name="filename">The file to get the directory of.</param>
        public static string GetDirectoryName(string filename)
        {
            return Device.File.DirectoryName(filename);
        }

        /// <summary>
        /// Returns the name of the file.
        /// </summary>
        /// <param name="filename">The file to get the name of.</param>
        public static string GetFileName(string filename)
        {
            return filename.Substring(filename.LastIndexOf(DirectorySeparatorChar) + 1);
        }

        /// <summary>
        /// Returns the name of the file.
        /// </summary>
        /// <param name="filename">The file to get the name of.</param>
        public static string GetFileNameWithoutExtension(string filename)
        {
            filename = GetFileName(filename);
            int i = filename.LastIndexOf('.');
            return filename.Remove(i, filename.Length - 1 - i);
        }

        /// <summary>
        /// Gets the directory separator character for the platform.
        /// </summary>
        public static char DirectorySeparatorChar
        {
            get
            {
                return Device.DirectorySeparatorChar;
            }
        }

        /// <summary>
        /// Gets the full path of a filename.
        /// </summary>
        /// <param name="filename">The name of the file to create a whole path.</param>
        /// <returns>The full path of the file.</returns>
        /// <remarks>
        /// If the file does not exist, the path is assumed to resolve to <see cref="Device.SessionDataPath"/>.<br/>
        /// If the file exists in the <see cref="Device.DataPath"/> or <see cref="Device.ApplicationPath"/>, the
        /// appropriate base path will be prepended.<br/>
        /// Fully-qualified paths will be returned as-is.
        /// </remarks>
        public static string GetFullPath(string filename)
        {
            if (!File.Exists(filename)) return Combine(Device.SessionDataPath, filename);

            string retval;
            if (File.Exists(retval = Combine(Device.DataPath, filename)) ||
                File.Exists(retval = Combine(Device.ApplicationPath, filename)))
            {
                return retval;
            }
            return filename;
        }
    }
}
