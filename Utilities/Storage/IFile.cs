using System.IO;

namespace MonoCross.Utilities.Storage
{
    /// <summary>
    ///Defines the MonoCross abstract file system utility. 
    /// </summary>
    public interface IFile
    {
        //Stream Create(string filename);
        //Stream Open( string filename );

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        byte[] Read( string filename );
        
        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="mode">The encryption mode to use when reading the file.</param>
        byte[] Read( string filename, EncryptionMode mode );
        
        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        byte[] Read( string filename, string key, byte[] salt );

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        string ReadString( string filename );
        
        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="mode">The encryption mode to use when reading the file.</param>
        string ReadString( string filename, EncryptionMode mode );
        
        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        string ReadString( string filename, string key, byte[] salt );

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        void Save( string filename, string contents );
        
        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        void Save( string filename, string contents, EncryptionMode mode );
        
        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        void Save( string filename, string contents, string key, byte[] salt );

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        void Save( string filename, Stream contents );
        
        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        void Save( string filename, Stream contents, EncryptionMode mode );
        
        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        void Save( string filename, Stream contents, string key, byte[] salt );

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        void Save( string filename, byte[] contents );

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        void Save( string filename, byte[] contents, EncryptionMode mode );
        
        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        void Save( string filename, byte[] contents, string key, byte[] salt );


        //byte[] ReadEncrypted( string filename, string key, byte[] salt );
        //string ReadStringEncrypted( string filename, string key, byte[] salt );
        //void SaveEncrypted( string filename, string contents, string key, byte[] salt );
        //void SaveEncrypted( string filename, Stream contents, string key, byte[] salt );
        //void SaveEncrypted( string filename, byte[] contents, string key, byte[] salt );

        //byte[] ReadClear( string filename );
        //string ReadStringClear( string filename );
        //void Save( string filename, string contents );
        //void Save( string filename, Stream contents );
        //void Save( string filename, byte[] contents );


        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        void Delete(string filename);

        /// <summary>
        /// Returns a value indicating whether or not the specified file exists.
        /// </summary>
        /// <param name="filename">The file to check for.</param>
        /// <returns><c>true</c> if the specified file exists; otherwise <c>false</c>.</returns>
        bool Exists(string filename);

        /// <summary>
        /// Moves the specified file to the specified destination.
        /// </summary>
        /// <param name="sourcefilename">The file to move.</param>
        /// <param name="destinationfilename">The destination of the file being moved.</param>
        void Move(string sourcefilename, string destinationfilename);

        /// <summary>
        /// Copies the specified file to the specified destination.
        /// </summary>
        /// <param name="sourcefilename">The file to copy.</param>
        /// <param name="destinationfilename">The destination of the file being copied.</param>
        void Copy( string sourcefilename, string destinationfilename );

        //void AppendClearText( string filename, string contents ); // no encryption saving
        /// <summary>
        /// Returns the character length of the specified file.
        /// </summary>
        /// <param name="filename">The file to get the length of.</param>
        long Length( string filename );

        /// <summary>
        /// Creates a directory with the specified name.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        void CreateDirectory(string directoryName);

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to delete.</param>
        void DeleteDirectory(string directoryName);

        /// <summary>
        /// Deletes the specified directory and optionally deletes all files and subdirectories within the directory.
        /// </summary>
        /// <param name='directoryName'>The directory to delete.</param>
        /// <param name='recursive'>Whether to delete all files and subdirectories within the directory.</param>
        void DeleteDirectory(string directoryName, bool recursive);

        /// <summary>
        /// Moves the specified directory to the specified destination.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to move.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being moved.</param>
        void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName);

        /// <summary>
        /// Copies the specified directory to the specified destination.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being copied.</param>
        void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName);

        /// <summary>
        /// Copies the specified directory to the specified destination.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being copied.</param>
        /// <param name="overwriteexisting">Whether to overwrite any directory that is already at the destination.</param>
        void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwriteexisting);

        /// <summary>
        /// Returns the names of the files that are inside of the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the files of.</param>
        string[] GetFileNames(string directoryName);

        /// <summary>
        /// Returns the names of subdirectories within the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the subdirectories of.</param>
        string[] GetDirectoryNames(string directoryName);

        /// <summary>
        /// Returns the name of the directory that contains the specified file.
        /// </summary>
        /// <param name="filename">The file to get the directory of.</param>
        string DirectoryName(string filename);

        /// <summary>
        /// Ensures that all directories in the specified path exist.
        /// </summary>
        /// <param name='filename'>The full path of the file or directory to check.</param>
        void EnsureDirectoryExists(string filename);
        
        /// <summary>
        /// Ensures that all directories in the specified path exist.
        /// </summary>
        /// <param name='filename'>The full path of the file or directory to check.</param>
        void EnsureDirectoryExistsForFile(string filename);
    }
}
