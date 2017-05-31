using System.IO;

namespace MonoCross.Utilities.Storage
{
    /// <summary>
    /// Represents a file system utility with no implementation.  This is compatible with all platforms and targets, and it is useful
    /// for when a concrete class is required but no implementation is necessary.
    /// </summary>
    public class NullFile : IFile
    {
        #region IFile Members

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        public byte[] Read( string filename )
        {
            return null; //            File.OpenWrite( fileName );
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        public string ReadString( string filename )
        {
            return null; //            File.OpenWrite( fileName );
        }

        /// <summary>
        /// Deletes the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public void Delete(string filename) { }

        /// <summary>
        /// Returns the character length of the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to get the length of.</param>
        public long Length(string filename) { return 0; }

        /// <summary>
        /// Moves the specified file to the specified destination.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="sourcefilename">The file to move.</param>
        /// <param name="destinationfilename">The destination of the file being moved.</param>
        public void Move(string sourcefilename, string destinationfilename) { }

        /// <summary>
        /// Copies the specified file to the specified destination.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="sourcefilename">The file to copy.</param>
        /// <param name="destinationfilename">The destination of the file being copied.</param>
        public void Copy(string sourcefilename, string destinationfilename) { }

        /// <summary>
        /// Creates a directory with the specified name.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        public void CreateDirectory(string directoryName) { }

        /// <summary>
        /// Deletes the specified directory.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="directoryName">The directory to delete.</param>
        public void DeleteDirectory(string directoryName) { }

        /// <summary>
        /// Deletes the specified directory and optionally deletes all subdirectories within the directory.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name='directoryName'>The directory to delete.</param>
        /// <param name='recursive'>Whether to delete all subdirectories within the directory.</param>
        public void DeleteDirectory(string directoryName, bool recursive) { }

        /// <summary>
        /// Moves the specified directory to the specified destination.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to move.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being moved.</param>
        public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName) { }

        /// <summary>
        /// Copies the specified directory to the specified destination.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being copied.</param>
        public void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
        }

        /// <summary>
        /// Copies the specified directory to the specified destination.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being copied.</param>
        /// <param name="overwriteexisting">Whether to overwrite any directory that is already at the destination.</param>
        public void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwriteexisting)
        {
        }

        /// <summary>
        /// Returns the names of the files that are inside of the specified directory.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="directoryName">The directory to get the files of.</param>
        public string[] GetFileNames(string directoryName)
        {
            return new string[] { };
        }

        /// <summary>
        /// Returns the names of subdirectories within the specified directory.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="directoryName">The directory to get the subdirectories of.</param>
        public string[] GetDirectoryNames(string directoryName)
        {
            return new string[] { };
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified file exists.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to check for.</param>
        /// <returns><c>true</c> if the specified file exists; otherwise <c>false</c>.</returns>
        public bool Exists(string filename)
        {
            return false;
        }

        /// <summary>
        /// Returns the name of the directory that contains the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to get the directory of.</param>
        public string DirectoryName(string filename)
        {
            return string.Empty;
        }

        /// <summary>
        /// Ensures that all directories in the specified path exist.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name='filename'>The full path of the file or directory to check.</param>
        public void EnsureDirectoryExists(string filename) { }

        /// <summary>
        /// Ensures that all directories in the specified path exist.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name='filename'>The full path of the file or directory to check.</param>
        public void EnsureDirectoryExistsForFile(string filename) { }

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        public void Save(string filename, string contents) { }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        public void Save(string filename, Stream contents) { }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        public void Save(string filename, byte[] contents) { }
        #endregion

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="mode">The encryption mode to use when reading the file.</param>
        public byte[] Read( string filename, EncryptionMode mode )
        {
            return null; 
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public byte[] Read( string filename, string key, byte[] salt )
        {
            return null; 
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="mode">The encryption mode to use when reading the file.</param>
        public string ReadString( string filename, EncryptionMode mode )
        {
            return null; 
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public string ReadString( string filename, string key, byte[] salt )
        {
            return null; 
        }

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        public void Save( string filename, string contents, EncryptionMode mode )
        {
        }

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public void Save( string filename, string contents, string key, byte[] salt )
        {
        }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        public void Save( string filename, Stream contents, EncryptionMode mode )
        {
        }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public void Save( string filename, Stream contents, string key, byte[] salt )
        {
        }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        public void Save( string filename, byte[] contents, EncryptionMode mode )
        {
        }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// Note that this does not actually do anything in a <see cref="NullFile"/> instance.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public void Save( string filename, byte[] contents, string key, byte[] salt )
        {
        }
    }
}
