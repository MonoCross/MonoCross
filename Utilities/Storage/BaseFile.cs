using System;
using System.IO;
using MonoCross.Utilities.Network;
using Path = System.IO.Path;

namespace MonoCross.Utilities.Storage
{
    /// <summary>
    /// Represents a base class for file system utilities.  This class is abstract.
    /// </summary>
    public abstract class BaseFile : IFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFile"/> class.
        /// </summary>
        public BaseFile()
        {
        }

        #region IFile Members

        #region Read() Methods

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <remarks>Internally calls the ReadClear protected virtual method.</remarks>
        public virtual byte[] Read(string filename)
        {
            return Read(filename, EncryptionMode.Default);
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="mode">The encryption mode to use when reading the file.</param>
        /// <remarks>Internally calls the ReadClear protected virtual method.</remarks>
        public virtual byte[] Read(string filename, EncryptionMode mode)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(filename);
            }

            DateTime dtMetric = DateTime.UtcNow;

            if (!Exists(filename) && !filename.StartsWith(Device.ApplicationPath))
                filename = Path.Combine(Device.ApplicationPath, filename);

            if (!Exists(filename))
                return null;

            byte[] bytes = null;

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    bytes = ReadClear(filename);
                    break;
                case EncryptionMode.Encryption:
                    bytes = Read(filename, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    if (Device.Encryption.Required)
                        bytes = Read(filename, Device.Encryption.Key, Device.Encryption.Salt);
                    else
                        bytes = ReadClear(filename);
                    break;
            }

            Device.Log.Metric(string.Format("BaseFile.Read: Mode: {0} File: {1} Time: {2:0} milliseconds", mode, filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return bytes;
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <remarks>Internally calls the ReadClear protected virtual method.</remarks>
        public virtual byte[] Read(string filename, string key, byte[] salt)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(filename);
            }

            DateTime dtMetric = DateTime.UtcNow;

            if (!Exists(filename))
                filename = Path.Combine(Device.ApplicationPath, filename);

            if (!Exists(filename) || Length(filename) == 0)
                return null;

            byte[] decrypted = null;

            try
            {
                byte[] bytes = ReadClear(filename);
                decrypted = Device.Encryption.DecryptBytes(bytes, key, salt);
            }
            catch (Exception ex)
            {
                Device.Log.Error("Error reading file: " + filename, ex);
                throw;
            }

            Device.Log.Metric(string.Format("BaseFile.Read(key,salt): File: {0} Time: {1:0} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return decrypted;
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        protected virtual byte[] ReadClear(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(filename);
            }

            byte[] retval = null;
            // FSTODO: capture and log metrics on file object.
            //if (filename.IndexOf("cache_index.xml") != -1)
            //    Device.Log.Debug(string.Format("Reading File: {0} - BaseFile:l92", filename));

            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            try
            {
                if (filename.IndexOf("cache_index.xml", StringComparison.Ordinal) != -1)
                {
                    Device.Log.Error(string.Format("Attempting Read File: {0} Complete - BaseFile:l100, tid:{1}", filename, threadId));
                }
                using (var file = File.OpenRead(filename))
                {
                    retval = new byte[file.Length];
                    file.Read(retval, 0, (int)file.Length);
                }
                if (filename.IndexOf("cache_index.xml", StringComparison.Ordinal) != -1)
                {
                    Device.Log.Error(string.Format("Reading File: {0} Complete - BaseFile:l104, tid:{1}", filename, threadId));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error Reading File: {0} - BaseFile:l99, tid:{1}", filename, threadId), ex);
            }
            return retval;
        }

        #endregion

        #region ReadString() Methods

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <remarks>Internally calls the ReadClear protected virtual method for encryption or the ReadStringClear virtual method for no encryption.</remarks>
        public virtual string ReadString(string filename)
        {
            return ReadString(filename, EncryptionMode.Default);
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="mode">The encryption mode to use when reading the file.</param>
        /// <remarks>Internally calls the ReadClear protected virtual method for encryption or the ReadStringClear virtual method for no encryption.</remarks>
        public virtual string ReadString(string filename, EncryptionMode mode)
        {
            DateTime dtMetric = DateTime.UtcNow;

            if (!Exists(filename))
                filename = Path.Combine(Device.ApplicationPath, filename);
            if (!Exists(filename))
                return null;

            string text = null;

            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    text = ReadStringClear(filename);
                    break;
                case EncryptionMode.Encryption:
                    text = ReadString(filename, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    if (Device.Encryption.Required)
                        text = ReadString(filename, Device.Encryption.Key, Device.Encryption.Salt);
                    else
                        text = ReadStringClear(filename);
                    break;
            }
            Device.Log.Metric(string.Format("BasicFile.ReadString: Mode: {0}  File: {1} Time: {2:0} milliseconds", mode, filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return text;
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <remarks>Internally calls the ReadClear protected virtual method.</remarks>
        public virtual string ReadString(string filename, string key, byte[] salt)
        {
            byte[] contents = Read(filename, key, salt);
            return NetworkUtils.ByteArrayToStr(contents);
        }

        /// <summary>
        /// Reads the specified file and returns its contents as a string.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        public virtual string ReadStringClear(string filename)
        {
            return File.OpenText(filename).ReadToEnd();
        }

        #endregion

        /// <summary>
        /// Returns the character length of the specified file.
        /// </summary>
        /// <param name="filename">The file to get the length of.</param>
        public virtual long Length(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            return fi.Length;
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filename">The file to delete.</param>
        public virtual void Delete(string filename)
        {
            if (Exists(filename))
                File.Delete(filename);
        }

        /// <summary>
        /// Moves the specified file to the specified destination.
        /// </summary>
        /// <param name="sourcefilename">The file to move.</param>
        /// <param name="destinationfilename">The destination of the file being moved.</param>
        public virtual void Move(string sourcefilename, string destinationfilename)
        {
            // Ensure that the target does not exist.
            Delete(destinationfilename);

            // trying copy+delete instead of move to solve intermittent file access error (MDT QC#4340).
            //File.Move(sourcefilename, destinationfilename);
            File.Copy(sourcefilename, destinationfilename, true);
            Delete(sourcefilename);
        }

        /// <summary>
        /// Copies the specified file to the specified destination.
        /// </summary>
        /// <param name="sourcefilename">The file to copy.</param>
        /// <param name="destinationfilename">The destination of the file being copied.</param>
        public virtual void Copy(string sourcefilename, string destinationfilename)
        {
            if (sourcefilename == null) { throw new ArgumentNullException("sourcefilename"); }
            else if (destinationfilename == null) { throw new ArgumentNullException("destinationfilename"); }

            try
            {
                File.Copy(sourcefilename, destinationfilename, true);
            }
            catch
            {
                File.Copy(Device.ApplicationPath.AppendPath(sourcefilename), destinationfilename, true);
            }
        }

        /// <summary>
        /// Creates a directory with the specified name.
        /// </summary>
        /// <param name="directoryName">The name of the directory to create.</param>
        public virtual void CreateDirectory(string directoryName)
        {
            Directory.CreateDirectory(directoryName);
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to delete.</param>
        public virtual void DeleteDirectory(string directoryName)
        {
            if (Directory.Exists(directoryName))
                Directory.Delete(directoryName, true);
        }

        /// <summary>
        /// Deletes the specified directory and optionally deletes all files and subdirectories within the directory.
        /// </summary>
        /// <param name='directoryName'>The directory to delete.</param>
        /// <param name='recursive'>Whether to delete all files and subdirectories within the directory.</param>
        public virtual void DeleteDirectory(string directoryName, bool recursive)
        {
            if (Directory.Exists(directoryName))
                Directory.Delete(directoryName, recursive);
        }

        /// <summary>
        /// Moves the specified directory to the specified destination.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to move.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being moved.</param>
        public virtual void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
            CopyDirectory(sourceDirectoryName, destinationDirectoryName, true);
            DeleteDirectory(sourceDirectoryName);
        }

        /// <summary>
        /// Copies the specified directory to the specified destination.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being copied.</param>
        public virtual void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
            CopyDirectory(sourceDirectoryName, destinationDirectoryName, true);
        }

        /// <summary>
        /// Copies the specified directory to the specified destination.
        /// </summary>
        /// <param name="sourceDirectoryName">The directory to copy.</param>
        /// <param name="destinationDirectoryName">The destination of the directory being copied.</param>
        /// <param name="overwriteExisting">Whether to overwrite any directory that is already at the destination.</param>
        public virtual void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwriteExisting)
        {
            if (sourceDirectoryName == null) { throw new ArgumentNullException("sourceDirectoryName", "Must specify source directory"); }
            else if (destinationDirectoryName == null) { throw new ArgumentNullException("destinationDirectoryName", "Must specify destination directory"); }


            // Make sure the source exists first
            //if (!Directory.Exists(sourceDirectoryName)) 
            //    return;

            // Create the destination folder if needed
            CreateDirectory(destinationDirectoryName);

#if !SILVERLIGHT
            // Copy the files and overwrite destination files if they already exist.
            foreach (string fls in Directory.GetFiles(sourceDirectoryName))
            {
                FileInfo flInfo = new FileInfo(fls);
                flInfo.CopyTo(Path.Combine(destinationDirectoryName, flInfo.Name), overwriteExisting);
            }

#endif
            // Copy all subfolders by calling CopyDirectory recursively
            foreach (string drs in GetDirectoryNames(sourceDirectoryName))
            {
                DirectoryInfo drInfo = new DirectoryInfo(drs);
                CopyDirectory(drs, Path.Combine(destinationDirectoryName, drInfo.Name), overwriteExisting);
            }
        }

        /// <summary>
        /// Returns the names of the files that are inside of the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the files of.</param>
        public abstract string[] GetFileNames(string directoryName);

        /// <summary>
        /// Returns the name of the directory that contains the specified file.
        /// </summary>
        /// <param name="filename">The file to get the directory of.</param>
        public virtual string DirectoryName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;
            return Path.GetDirectoryName(filename);
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified file exists.
        /// </summary>
        /// <param name="filename">The file to check for.</param>
        /// <returns><c>true</c> if the specified file exists; otherwise <c>false</c>.</returns>
        public virtual bool Exists(string filename)
        {
            if (filename == null)
            {
                return false;
            }

            return Directory.Exists(filename) || File.Exists(filename);
        }

        /// <summary>
        /// Ensures that all directories in the specified path exist.
        /// </summary>
        /// <param name='filename'>The full path of the file or directory to check.  If passing a directory, a trailing slash is required.</param>
        [Obsolete("Use EnsureDirectoryExistsForFile instead.")]
        public virtual void EnsureDirectoryExists(string filename)
        {
            EnsureDirectoryExistsForFile(filename);
        }

        /// <summary>
        /// Ensures that all directories in the specified path exist.
        /// </summary>
        /// <param name='filename'>The full path of the file or directory to check.  If passing a directory, a trailing slash is required.</param>
        public virtual void EnsureDirectoryExistsForFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            string dir = DirectoryName(filename);

            if (string.IsNullOrEmpty(dir.Trim()) || Exists(dir)) return;

            // FSTODO: log directory creation.
            if (filename.IndexOf("cache_index.xml", StringComparison.Ordinal) != -1)
            {
                var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                Device.Log.Debug(string.Format("Creating File: {0} - BaseFile:l535, tid:{1}", filename, threadId));
            }
            CreateDirectory(dir);
        }

        /// <summary>
        /// Returns the names of subdirectories within the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the subdirectories of.</param>
        public abstract string[] GetDirectoryNames(string directoryName);

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        public virtual void Save(string filename, string contents)
        {
            Save(filename, contents, EncryptionMode.Default);
        }

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        public virtual void Save(string filename, string contents, EncryptionMode mode)
        {
            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    SaveClear(filename, contents);
                    break;
                case EncryptionMode.Encryption:
                    Save(filename, contents, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    if (Device.Encryption.Required)
                        Save(filename, contents, Device.Encryption.Key, Device.Encryption.Salt);
                    else
                        SaveClear(filename, contents);
                    break;
            }
        }

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public virtual void Save(string filename, string contents, string key, byte[] salt)
        {
            // TODO: determine if contents can be null
            // null contents is valid on Windows and Droid
            //if (contents == null)
            //{
            //    throw new ArgumentNullException("contents", "contents cannot be null");
            //}
            byte[] bytes = NetworkUtils.StrToByteArray(contents);
            Save(filename, bytes, key, salt);
        }

        /// <summary>
        /// Writes the specified contents to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The contents to write to the file.</param>
        protected virtual void SaveClear(string filename, string contents)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename", "must provide a file name");
            }
            EnsureDirectoryExistsForFile(filename);
            var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(contents));
            SaveClear(filename, stream);
            stream.Close();
            stream.Dispose();
        }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        public virtual void Save(string filename, Stream contents)
        {
            Save(filename, contents, EncryptionMode.Default);
        }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        public virtual void Save(string filename, Stream contents, EncryptionMode mode)
        {
            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    SaveClear(filename, contents);
                    break;
                case EncryptionMode.Encryption:
                    Save(filename, contents, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    if (Device.Encryption.Required)
                        Save(filename, contents, Device.Encryption.Key, Device.Encryption.Salt);
                    else
                        SaveClear(filename, contents);
                    break;
            }
        }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public virtual void Save(string filename, Stream contents, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename", "must provide a file name");
            }

            if (string.IsNullOrEmpty(key) || salt == null)
            {
                throw new ArgumentException("must provide key and salt");
            }
            EnsureDirectoryExistsForFile(filename);

            DateTime dtMetric = DateTime.UtcNow;

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filename, FileMode.Create);
                Device.Encryption.EncryptStream(contents, fileStream, key, salt);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            Device.Log.Metric(string.Format("BasicFile.Save(stream, key, salt): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Writes the contents of the specified stream to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The stream containing the contents to write to the file.</param>
        protected virtual void SaveClear(string filename, Stream contents)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename", "must provide a file name");
            }

            EnsureDirectoryExistsForFile(filename);

            DateTime dtMetric = DateTime.UtcNow;

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            using (BinaryReader binaryReader = new BinaryReader(contents))
            {
                try
                {
                    // process through stream in small chunks to keep peak memory usage down.
                    byte[] bytes = binaryReader.ReadBytes(8192);
                    while (bytes.Length > 0)
                    {
                        binaryWriter.Write(bytes);
                        bytes = binaryReader.ReadBytes(8192);
                    }
                }
                finally
                {
                    binaryWriter.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                    binaryReader.Close();
                }
                Device.Log.Metric(string.Format("BasicFile.SaveClear(stream): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
            }
        }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        public virtual void Save(string filename, byte[] contents)
        {
            Save(filename, contents, EncryptionMode.Default);
        }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        /// <param name="mode">The encryption mode to use when writing to the file.</param>
        public virtual void Save(string filename, byte[] contents, EncryptionMode mode)
        {
            switch (mode)
            {
                case EncryptionMode.NoEncryption:
                    SaveClear(filename, contents);
                    break;
                case EncryptionMode.Encryption:
                    Save(filename, contents, Device.Encryption.Key, Device.Encryption.Salt);
                    break;
                case EncryptionMode.Default:
                    if (Device.Encryption.Required)
                        Save(filename, contents, Device.Encryption.Key, Device.Encryption.Salt);
                    else
                        SaveClear(filename, contents);
                    break;
            }
        }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public virtual void Save(string filename, byte[] contents, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename", "must provide a file name");
            }
            if (string.IsNullOrEmpty(key) || salt == null)
            {
                throw new ArgumentException("must provide key and salt");
            }

            EnsureDirectoryExistsForFile(filename);

            DateTime dtMetric = DateTime.UtcNow;

            FileStream fileStream = null;
            MemoryStream plainStream = null;

            try
            {
                fileStream = new FileStream(filename, FileMode.Create);
                // TODO: verify passing null into MemoryStream ctor is valid on all platforms
                //if (contents == null) contents = new byte[0];
                plainStream = new MemoryStream(contents);

                Device.Encryption.EncryptStream(plainStream, fileStream, key, salt);
            }
            catch (Exception ex)
            {
                Device.Log.Error("Error writing file: " + filename, ex);
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (plainStream != null)
                {
                    plainStream.Close();
                    plainStream.Dispose();
                }
            }
            Device.Log.Metric(string.Format("BasicFile.Save(byte[],key,salt): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Writes the contents of the specified byte array to the specified file.
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        /// <param name="contents">The byte array containing the contents to write to the file.</param>
        protected virtual void SaveClear(string filename, byte[] contents)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename", "must provide a file name");
            }
            //FSTODO: add debug metrics.
            EnsureDirectoryExistsForFile(filename);

            DateTime dtMetric = DateTime.UtcNow;

            FileStream fileStream = null;
            try
            {
                var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

                if (filename.IndexOf("cache_index.xml", StringComparison.Ordinal) != -1)
                    Device.Log.Debug(string.Format("Creating File Stream: {0} - BaseFile:l543, tid:{1}", filename, threadId));
                fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);

                if (filename.IndexOf("cache_index.xml", StringComparison.Ordinal) != -1)
                    Device.Log.Debug(string.Format("Writing File Stream: {0} - BaseFile:l546, tid:{1}", filename, threadId));
                if (contents != null)
                    fileStream.Write(contents, 0, contents.Length);
            }
            finally
            {
                if (fileStream != null)
                {
                    if (filename.IndexOf("cache_index.xml", StringComparison.Ordinal) != -1)
                        Device.Log.Debug(string.Format("Disposing File Stream: {0} - BaseFile:l563", filename));
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            Device.Log.Metric(string.Format("BasicFile.SaveClear(byte[]): File: {0} Time: {1:0} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        #endregion
    }
}
