using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;

namespace MonoCross.Utilities.Storage
{
    /// <summary>
    /// Provides Isolated Storage File functionality for Silverlight-based targets.
    /// </summary>
    public class SLFile : BaseFile
    {
        internal readonly IsolatedStorageFile _store = IsolatedStorageFile.GetUserStoreForApplication();

        /// <summary>
        /// Default constructor for SLFile.
        /// </summary>
        public SLFile()
        {
        }

        public override byte[] Read(string filename)
        {
            return Read(filename, EncryptionMode.Default);
        }

        public override byte[] Read(string filename, EncryptionMode mode)
        {
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
            return bytes;
        }

        public override byte[] Read(string filename, string key, byte[] salt)
        {
            if (!Exists(filename) || Length(filename) == 0)
                return null;
            byte[] bytes = ReadClear(filename);
            byte[] decrypted = Device.Encryption.DecryptBytes(bytes, key, salt);
            return decrypted;
        }

        public override string ReadStringClear(string filename)
        {
            string retval = null;

            if (!filename.IsRemotePath() && (_store.FileExists(filename) || _store.DirectoryExists(filename)))
            {
                IsolatedStorageFileStream fileStream = null;
                try
                {
                    fileStream = _store.OpenFile(filename, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        retval = streamReader.ReadToEnd();
                        streamReader.Close();
                        streamReader.Dispose();
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
            }
            else
            {
                Stream stream = null;
                try
                {
                    var resourceStream = Application.GetResourceStream(new Uri(filename.Replace("\\", "/"), UriKind.Relative));
                    if (resourceStream != null)
                    {
                        stream = resourceStream.Stream;
                        using (var streamReader = new StreamReader(stream))
                        {
                            retval = streamReader.ReadToEnd();
                            streamReader.Close();
                            streamReader.Dispose();
                        }
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }

            return retval;
        }

        protected override byte[] ReadClear(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return null;
            }

            byte[] bytes = null;

            if (!filename.IsRemotePath() && (_store.FileExists(filename) || _store.DirectoryExists(filename)))
            {
                IsolatedStorageFileStream fileStream = null;
                try
                {
                    fileStream = _store.OpenFile(filename, FileMode.Open);
                    bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, Convert.ToInt32(fileStream.Length));
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
            }
            else
            {
                Stream stream = null;
                try
                {
                    var resourceStream = Application.GetResourceStream(new Uri(filename.Replace("\\", "/"), UriKind.Relative));
                    if (resourceStream != null)
                    {
                        stream = resourceStream.Stream;
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }

            return (bytes == null || bytes.Length == 0) ? null : bytes;
        }

        public override long Length(string fileName)
        {
            Stream stream = null;

            try
            {
                if (_store.FileExists(fileName))
                {
                    stream = _store.OpenFile(fileName, FileMode.Open);
                }
                else if (File.Exists(fileName))
                {
                    stream = File.Open(fileName, FileMode.Open);
                }

                if (stream != null)
                {
                    return stream.Length;
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return 0;
        }

        public override void Delete(string fileName)
        {
            if (!fileName.IsRemotePath() && _store.FileExists(fileName))
                _store.DeleteFile(fileName);
        }

        public override void Move(string sourceFileName, string destinationFileName)
        {
            Delete(destinationFileName);
            _store.MoveFile(sourceFileName, destinationFileName);
        }

        public override void Copy(string sourceFileName, string destinationFileName)
        {
            if (!sourceFileName.IsRemotePath() && _store.FileExists(sourceFileName))
            {
                _store.CopyFile(sourceFileName, destinationFileName);
            }
            else if (File.Exists(sourceFileName))
            {
                using (var stream = File.Open(sourceFileName, FileMode.Open))
                using (var isostream = _store.OpenFile(destinationFileName, FileMode.OpenOrCreate))
                {
                    stream.CopyTo(isostream);
                    isostream.Flush();
                }
            }
        }

        public override void CreateDirectory(string directoryName)
        {
            _store.CreateDirectory(directoryName);
        }

        public override void DeleteDirectory(string directoryName)
        {
            if (!_store.DirectoryExists(directoryName)) return;
            var pattern = directoryName + @"\*";

            var files = _store.GetFileNames(pattern);
            foreach (var fName in files)
            {
                _store.DeleteFile(Path.Combine(directoryName, fName));
            }

            var dirs = _store.GetDirectoryNames(pattern);
            foreach (var dName in dirs)
            {
                DeleteDirectory(Path.Combine(directoryName, dName));
            }

            _store.DeleteDirectory(directoryName);
        }

        public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
            _store.MoveDirectory(sourceDirectoryName, destinationDirectoryName);
        }

        public override void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwriteexisting)
        {
            // Create the destination folder if needed
            CreateDirectory(destinationDirectoryName);

            if (_store.DirectoryExists(sourceDirectoryName))
            {
                // Copy the files and overwrite destination files if they already exist.
                foreach (string file in _store.GetFileNames(Path.Combine(sourceDirectoryName, "*")))
                    _store.CopyFile(Path.Combine(sourceDirectoryName, file), Path.Combine(destinationDirectoryName, file), overwriteexisting);

                // Copy all subfolders by calling CopyDirectory recursively
                foreach (string subDirectory in _store.GetDirectoryNames(Path.Combine(sourceDirectoryName, "*")))
                    CopyDirectory(Path.Combine(sourceDirectoryName, subDirectory), Path.Combine(destinationDirectoryName, subDirectory), overwriteexisting);
            }
            else if (Directory.Exists(sourceDirectoryName))
            {
                foreach (string file in MonoCross.Utilities.Compatibility.Directory.GetFiles(sourceDirectoryName))
                {
                    Copy(Path.Combine(sourceDirectoryName, file), Path.Combine(destinationDirectoryName, file));
                }

                foreach (string subDirectory in MonoCross.Utilities.Compatibility.Directory.GetDirectories(sourceDirectoryName))
                {
                    CopyDirectory(Path.Combine(sourceDirectoryName, subDirectory), Path.Combine(destinationDirectoryName, subDirectory));
                }
            }
        }

        public override string[] GetFileNames(string directoryName)
        {
            try
            {
                return _store.GetFileNames(directoryName + "*").Select(f => Path.Combine(directoryName, f)).ToArray();
            }
            catch (DirectoryNotFoundException)
            {
#if WINDOWS_PHONE
                return Directory.GetFiles(directoryName);
#else
                return Directory.EnumerateFiles(directoryName).ToArray();
#endif
            }
        }

        public override bool Exists(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileName.IsRemotePath())
            {
                return false;
            }

            return File.Exists(fileName) || Directory.Exists(fileName) || _store.DirectoryExists(fileName) || _store.FileExists(fileName);
        }

        public override void EnsureDirectoryExistsForFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            string dir = DirectoryName(filename);

            if (!string.IsNullOrEmpty(dir.Trim()) && !_store.DirectoryExists(dir))
            {
                CreateDirectory(dir);
            }
        }

        public override string[] GetDirectoryNames(string directoryName)
        {
            try
            {
                return _store.GetDirectoryNames(directoryName + "*");
            }
            catch (DirectoryNotFoundException)
            {
#if WINDOWS_PHONE
                return Directory.GetDirectories(directoryName);
#else
                return Directory.EnumerateDirectories(directoryName).ToArray();
#endif
            }
        }

        protected override void SaveClear(string filename, Stream contents)
        {
            EnsureDirectoryExistsForFile(filename);
            Delete(filename);

            if (contents == null || (contents.Length > _store.AvailableFreeSpace &&
                !IncreaseStorage(contents.Length + (_store.Quota - _store.AvailableFreeSpace))))
                return;

            IsolatedStorageFileStream fileStream = null;
            try
            {
                fileStream = _store.OpenFile(filename, FileMode.CreateNew);
                int read;
                var bytes = new byte[256];
                while ((read = contents.Read(bytes, 0, 256)) > 0)
                    fileStream.Write(bytes, 0, read);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        public override void Save(string filename, Stream contents, string key, byte[] salt)
        {
            EnsureDirectoryExistsForFile(filename);
            Delete(filename);

            if (contents == null || (contents.Length > _store.AvailableFreeSpace &&
                !IncreaseStorage(contents.Length + (_store.Quota - _store.AvailableFreeSpace))))
                return;

            IsolatedStorageFileStream fileStream = null;
            try
            {
                fileStream = _store.OpenFile(filename, FileMode.CreateNew);
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
        }

        protected override void SaveClear(string filename, string contents)
        {
            EnsureDirectoryExistsForFile(filename);
            Delete(filename);

            if (contents == null || (contents.Length > _store.AvailableFreeSpace &&
                !IncreaseStorage(contents.Length + (_store.Quota - _store.AvailableFreeSpace))))
                return;

            IsolatedStorageFileStream fileStream = null;
            try
            {
                fileStream = _store.OpenFile(filename, FileMode.CreateNew);
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(contents);
                    writer.Close();
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        protected override void SaveClear(string filename, byte[] contents)
        {
            EnsureDirectoryExistsForFile(filename);
            Delete(filename);

            if (contents == null || (contents.Length > _store.AvailableFreeSpace &&
                !IncreaseStorage(contents.Length + (_store.Quota - _store.AvailableFreeSpace))))
                return;

            IsolatedStorageFileStream fileStream = null;
            try
            {
                fileStream = _store.OpenFile(filename, FileMode.CreateNew);
                fileStream.Write(contents, 0, contents.Length);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        public override void Save(string filename, byte[] contents, string key, byte[] salt)
        {
            EnsureDirectoryExistsForFile(filename);
            Delete(filename);

            if (contents == null || (contents.Length > _store.AvailableFreeSpace &&
                !IncreaseStorage(contents.Length + (_store.Quota - _store.AvailableFreeSpace))))
                return;

            IsolatedStorageFileStream fileStream = null;
            MemoryStream plainStream = null;
            try
            {
                fileStream = _store.OpenFile(filename, FileMode.CreateNew);
                plainStream = new MemoryStream(contents);
                Device.Encryption.EncryptStream(plainStream, fileStream, key, salt);
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
        }

        internal virtual bool IncreaseStorage(long spaceRequest)
        {
            //increase quota by megabytes
            const long mb = 1048576;
            long count = 1;
            while (spaceRequest > mb * count)
                count++;

            try
            {
                return _store.IncreaseQuotaTo((mb * count) + _store.Quota);
            }
            catch
            {
                return false;
            }
        }
    }
}
