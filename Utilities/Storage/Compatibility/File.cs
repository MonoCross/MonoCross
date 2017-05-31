using System.IO;
using MonoCross.Utilities;

namespace MonoCross.Utilities.Compatibility
{
    /// <summary>
    /// Static helper class for working with files.
    /// </summary>
    public static class File
    {
        /// <summary>
        /// Returns a value indicating whether or not the specified file exists.
        /// </summary>
        /// <param name="filename">The file to check for.</param>
        /// <returns><c>true</c> if the specified file exists; otherwise <c>false</c>.</returns>
        public static bool Exists(string filename)
        {
            return Device.File.Exists(filename);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public static void Delete(string filename)
        {
            Device.File.Delete(filename);
        }

        /// <summary>
        /// Moves the specified file to the specified destination.
        /// </summary>
        /// <param name="sourcefilename">The file to move.</param>
        /// <param name="destinationfilename">The destination of the file being moved.</param>
        public static void Move(string sourcefilename, string destinationfilename)
        {
            Device.File.Move(sourcefilename, destinationfilename);
        }

        /// <summary>
        /// Opens the specified file as a read-only <see cref="Stream"/>.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Stream OpenRead(string fileName)
        {
            return new MemoryStream(Device.File.Read(fileName, EncryptionMode.NoEncryption));
        }
    }
}
