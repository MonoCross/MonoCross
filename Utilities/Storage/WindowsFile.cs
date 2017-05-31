using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoCross.Utilities.Networking;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MonoCross.Utilities.Storage
{
    public class WindowsFile : IFile
    {
        /// <summary>
        /// Reads the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public byte[] Read(string filename)
        {
            return Read(filename, EncryptionMode.Default);
        }

        /// <summary>
        /// Reads the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The encryption mode to be used.</param>
        /// <returns></returns>
        public byte[] Read(string filename, EncryptionMode mode)
        {
            DateTime dtMetric = DateTime.UtcNow;

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

            Device.Log.Metric(string.Format("BaseFile.Read: Mode: {0} File: {1} Time: {2} milliseconds", mode, filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return bytes;
        }

        /// <summary>
        /// Reads the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public byte[] Read(string filename, string key, byte[] salt)
        {
            DateTime dtMetric = DateTime.UtcNow;

            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            if (!Exists(filename) || Length(filename) == 0)
                return null;

            byte[] bytes;

            Stream fileStream = null;
            MemoryStream decryptStream = null;

            try
            {
                // Create the streams used for decryption.
                fileStream = StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync().OpenStreamForReadAsync().Result;
                decryptStream = new MemoryStream();

                Device.Encryption.DecryptStream(fileStream, decryptStream, key, salt);

                Device.Log.Metric(string.Format("BasicFile.Read(key,salt): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

                bytes = decryptStream.ToArray();

                return bytes;
            }

            catch (Exception cexc)
            {
                if (cexc.Message.Contains("Bad PKCS7 padding") || cexc.Message.Contains("Padding is invalid and cannot be removed"))
                {
                    // attempt to deserialize file with no encryption.
                    bytes = this.Read(filename, EncryptionMode.NoEncryption);
                    return bytes;
                }
                else
                {
                    Device.Log.Error("error in file read", cexc);
                    var exc = new Exception("error in file read", cexc);
                    exc.Data.Add("filename", filename);
                    throw exc;
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
                if (decryptStream != null)
                {
                    decryptStream.Dispose();
                }
            }
        }

        protected virtual byte[] ReadClear(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            var file = StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync();
            var buffer = FileIO.ReadBufferAsync(file).AsTask().RunSync();
            var dataReader = DataReader.FromBuffer(buffer);
            var retval = new byte[buffer.Length];
            dataReader.ReadBytes(retval);
            return retval;
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public string ReadString(string filename)
        {
            return ReadString(filename, EncryptionMode.Default);
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The encryption mode to be used.</param>
        /// <returns></returns>
        public string ReadString(string filename, EncryptionMode mode)
        {
            DateTime dtMetric = DateTime.UtcNow;

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
            Device.Log.Metric(string.Format("BasicFile.ReadString: Mode: {0}  File: {1} Time: {2} milliseconds", mode, filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));

            return text;
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public string ReadString(string filename, string key, byte[] salt)
        {
            byte[] contents = Read(filename, key, salt);
            return NetworkUtils.ByteArrayToStr(contents);
        }

        public virtual string ReadStringClear(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            var file = StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync();
            var buffer = FileIO.ReadBufferAsync(file).AsTask().RunSync();
            var dataReader = DataReader.FromBuffer(buffer);
            return dataReader.ReadString(buffer.Length);
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        public void Save(string filename, string contents)
        {
            Save(filename, contents, EncryptionMode.Default);
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="mode">The encryption mode to be used.</param>
        public void Save(string filename, string contents, EncryptionMode mode)
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

        protected virtual void SaveClear(string filename, string contents)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            EnsureDirectoryExistsForFile(filename);
            var name = Path.GetFileName(filename);
            string folderName = Path.GetDirectoryName(filename);
            if (folderName == string.Empty)
                folderName = Device.SessionDataPath;
            var folder = StorageFolder.GetFolderFromPathAsync(folderName).AsTask().RunSync();
            StorageFile.CreateStreamedFileAsync(name, async output =>
            {
                using (output)
                using (var writer = new DataWriter(output))
                {
                    writer.WriteString(contents);
                    try
                    {
                        await writer.StoreAsync();
                    }
                    catch (Exception)
                    {
                        output.FailAndClose(StreamedFileFailureMode.Failed);
                    }
                }
            }, null).AsTask().RunSync().CopyAsync(folder, name, NameCollisionOption.ReplaceExisting).AsTask().RunSync();
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        public void Save(string filename, string contents, string key, byte[] salt)
        {
            byte[] bytes = NetworkUtils.StrToByteArray(contents);
            Save(filename, bytes, key, salt);
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        public void Save(string filename, Stream contents)
        {
            Save(filename, contents, EncryptionMode.Default);
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="mode">The encryption mode to be used.</param>
        public void Save(string filename, Stream contents, EncryptionMode mode)
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
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        public void Save(string filename, Stream contents, string key, byte[] salt)
        {
            var outStream = new MemoryStream();
            Device.Encryption.EncryptStream(contents, outStream, key, salt);
            SaveClear(filename, outStream);
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        public void Save(string filename, byte[] contents)
        {
            Save(filename, contents, EncryptionMode.Default);
        }

        /// <summary>
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="mode">The encryption mode to be used.</param>
        public void Save(string filename, byte[] contents, EncryptionMode mode)
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
        /// Saves the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        public void Save(string filename, byte[] contents, string key, byte[] salt)
        {
            var plainStream = new MemoryStream(contents);
            var outStream = new MemoryStream();
            Device.Encryption.EncryptStream(plainStream, outStream, key, salt);
            SaveClear(filename, outStream);
        }

        protected virtual void SaveClear(string filename, byte[] contents)
        {
            SaveAsync(filename, contents);
        }

        protected virtual void SaveClear(string filename, Stream contents)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = contents.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                SaveAsync(filename, ms.ToArray());
            }
        }

        private async void SaveAsync(string filename, byte[] contents)
        {
            DateTime dtMetric = DateTime.UtcNow;

            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            EnsureDirectoryExistsForFile(filename);
            var folder = await StorageFolder.GetFolderFromPathAsync(DirectoryName(filename));
            StorageFile file = await folder.CreateFileAsync(Path.GetFileName(filename), CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (IOutputStream outputStream = fileStream.GetOutputStreamAt(0))
                {
                    using (DataWriter dataWriter = new DataWriter(outputStream))
                    {
                        dataWriter.WriteBytes(contents);
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }
                    await outputStream.FlushAsync();
                }
            }
            Device.Log.Metric(string.Format("BasicFile.SaveClear(stream): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Deletes the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void Delete(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            if (Exists(filename))
            {
                StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync().DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().RunSync();
            }
        }

        /// <summary>
        /// Determines whether the specified filename exists.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public bool Exists(string filename)
        {
            //Exists implementation per http://social.msdn.microsoft.com/Forums/en-US/winappswithcsharp/thread/1eb71a80-c59c-4146-aeb6-fefd69f4b4bb
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            filename = filename.Replace('/', '\\');
            try
            {
                StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync();
                return true;
            }
            catch (Exception)
            {
                //Nonexistent file
            }

            try
            {
                StorageFolder.GetFolderFromPathAsync(filename).AsTask().RunSync();
                return true;
            }
            catch (Exception)
            {
                //Nonexistent folder
            }

            filename = Path.Combine(Device.ApplicationPath, filename);
            try
            {
                StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync();
                return true;
            }
            catch (Exception)
            {
                //Nonexistent folder
            }

            try
            {
                StorageFolder.GetFolderFromPathAsync(filename).AsTask().RunSync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Moves the specified sourcefilename.
        /// </summary>
        /// <param name="sourcefilename">The sourcefilename.</param>
        /// <param name="destinationfilename">The destinationfilename.</param>
        public void Move(string sourcefilename, string destinationfilename)
        {
            sourcefilename = Path.IsPathRooted(sourcefilename) ? sourcefilename : Path.Combine(Device.ApplicationPath, sourcefilename);
            var source = StorageFile.GetFileFromPathAsync(sourcefilename).AsTask().RunSync();
            var dest = StorageFolder.GetFolderFromPathAsync(DirectoryName(destinationfilename)).AsTask().RunSync();
            source.MoveAsync(dest, Path.GetFileName(destinationfilename), NameCollisionOption.ReplaceExisting).AsTask().RunSync();
        }

        /// <summary>
        /// Copies the specified sourcefilename.
        /// </summary>
        /// <param name="sourcefilename">The sourcefilename.</param>
        /// <param name="destinationfilename">The destinationfilename.</param>
        public void Copy(string sourcefilename, string destinationfilename)
        {
            sourcefilename = Path.IsPathRooted(sourcefilename) ? sourcefilename : Path.Combine(Device.ApplicationPath, sourcefilename);
            var source = StorageFile.GetFileFromPathAsync(sourcefilename).AsTask().RunSync();
            var dest = StorageFolder.GetFolderFromPathAsync(DirectoryName(destinationfilename)).AsTask().RunSync();
            source.CopyAsync(dest, Path.GetFileName(destinationfilename), NameCollisionOption.ReplaceExisting).AsTask().RunSync();
        }

        /// <summary>
        /// Returns the length the specified filename in bytes.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public long Length(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            var fi = StorageFile.GetFileFromPathAsync(filename).AsTask().RunSync();
            return FileIO.ReadBufferAsync(fi).AsTask().RunSync().Length;
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        public void CreateDirectory(string directoryName)
        {
            if (Exists(directoryName)) return;
            // I don't know why, but exceptions are thrown without this
            EnsureDirectoryExistsForFile(directoryName);

            directoryName = Path.IsPathRooted(directoryName) ? directoryName : Path.Combine(Device.ApplicationPath, directoryName);
            var names = new Stack<string>(new[] { directoryName }.AsEnumerable());

            StorageFolder folder = null;
            while (folder == null)
            {
                try
                {
                    folder = StorageFolder.GetFolderFromPathAsync(names.Peek()).AsTask().RunSync();
                }
                catch (FileNotFoundException)
                {
                    names.Push(DirectoryName(names.Peek()));
                }
                catch (Exception e)
                {
                    Device.Log.Error(e);
                    return;
                }
            }
            names.Pop();
            while (names.Any())
            {
                folder = folder.CreateFolderAsync(Path.GetFileName(names.Pop())).AsTask().RunSync();
            }
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        public void DeleteDirectory(string directoryName)
        {
            DeleteDirectory(directoryName, false);
        }

        /// <summary>
        /// Deletes a directory and optionally deletes all subdirectories.
        /// </summary>
        /// <param name='directoryName'>
        /// Name of the directory.
        /// </param>
        /// <param name='recursive'>
        /// <c>true</c> to delete all subdirectories and files; otherwise <c>false</c>.
        /// </param>
        public async void DeleteDirectory(string directoryName, bool recursive)
        {
            if (directoryName == null) return;
            directoryName = Path.IsPathRooted(directoryName) ? directoryName : Path.Combine(Device.ApplicationPath, directoryName);
            if (!Exists(directoryName)) return;
            var folder = StorageFolder.GetFolderFromPathAsync(directoryName).AsTask().RunSync();
            if (!recursive && folder.GetItemsAsync().AsTask().RunSync().Any()) return;
            await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        /// <summary>
        /// Moves the directory.
        /// </summary>
        /// <param name="sourceDirectoryName">Name of the source directory.</param>
        /// <param name="destinationDirectoryName">Name of the destination directory.</param>
        public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
            CopyDirectory(sourceDirectoryName, destinationDirectoryName, true);
            DeleteDirectory(sourceDirectoryName, true);
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="sourceDirectoryName">Name of the source directory.</param>
        /// <param name="destinationDirectoryName">Name of the destination directory.</param>
        public void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName)
        {
            CopyDirectory(sourceDirectoryName, destinationDirectoryName, false);
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="sourceDirectoryName">Name of the source directory.</param>
        /// <param name="destinationDirectoryName">Name of the destination directory.</param>
        /// <param name="overwriteexisting">if set to <c>true</c> [overwriteexisting].</param>
        public void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwriteexisting)
        {
            sourceDirectoryName = Path.IsPathRooted(sourceDirectoryName) ? sourceDirectoryName : Path.Combine(Device.ApplicationPath, sourceDirectoryName);

            // Create the destination folder if needed
            CreateDirectory(destinationDirectoryName);

            // Make sure the source exists first
            if (!Exists(sourceDirectoryName)) return;

            // Copy the files and overwrite destination files if they already exist.
            var source = StorageFolder.GetFolderFromPathAsync(sourceDirectoryName).AsTask().RunSync();
            var dest = StorageFolder.GetFolderFromPathAsync(destinationDirectoryName).AsTask().RunSync();
            foreach (var fls in source.GetFilesAsync().AsTask().RunSync())
            {
                try
                {
                    fls.CopyAsync(dest, fls.Name, overwriteexisting ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists).AsTask().RunSync();
                }
                catch (Exception e)
                {
                    Device.Log.Warn(e);
                }
            }

            // Copy all subfolders by calling CopyDirectory recursively
            foreach (string drs in GetDirectoryNames(sourceDirectoryName))
            {
                var drInfo = source.GetFolderAsync(drs).AsTask().RunSync();
                CopyDirectory(drInfo.Path, Path.Combine(destinationDirectoryName, drInfo.Name), overwriteexisting);
            }
        }

        /// <summary>
        /// Gets the file names.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <returns></returns>
        public string[] GetFileNames(string directoryName)
        {
            directoryName = Path.IsPathRooted(directoryName) ? directoryName : Path.Combine(Device.ApplicationPath, directoryName);
            var dir = StorageFolder.GetFolderFromPathAsync(directoryName).AsTask().RunSync();
            return dir.GetFilesAsync().AsTask().RunSync().Select(file => file.Path).ToArray();
        }

        /// <summary>
        /// Gets the directory names.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <returns></returns>
        public string[] GetDirectoryNames(string directoryName)
        {
            directoryName = Path.IsPathRooted(directoryName) ? directoryName : Path.Combine(Device.ApplicationPath, directoryName);
            var dir = StorageFolder.GetFolderFromPathAsync(directoryName).AsTask().RunSync();
            return dir.GetFoldersAsync().AsTask().RunSync().Select(file => file.Name).ToArray();
        }

        /// <summary>
        /// Returns the directory name for the filename specified.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public string DirectoryName(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            return Path.GetDirectoryName(filename);
        }

        /// <summary>
        /// Ensures the directory exists.
        /// </summary>
        /// <param name="filename">The filename.</param>
        [Obsolete("Use EnsureDirectoryExistsForFile instead.")]
        public void EnsureDirectoryExists(string filename)
        {
            EnsureDirectoryExistsForFile(filename);
        }

        /// <summary>
        /// Ensures the directory exists.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void EnsureDirectoryExistsForFile(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            if (string.IsNullOrEmpty(filename))
                return;

            string dir = DirectoryName(filename);

            if (!Exists(dir))
                CreateDirectory(dir);
        }

        public static bool ImageExists(string filename)
        {
            filename = Path.IsPathRooted(filename) ? filename : Path.Combine(Device.ApplicationPath, filename);
            if (Device.File.Exists(filename))
                return true;

            int? scale = null;
            switch (DisplayInformation.GetForCurrentView().ResolutionScale)
            {
                case ResolutionScale.Scale100Percent:
                    scale = 100;
                    break;
                case ResolutionScale.Scale140Percent:
                    scale = 140;
                    break;
                case ResolutionScale.Scale180Percent:
                    scale = 180;
                    break;
            }

            if (scale.HasValue)
            {
                return Device.File.Exists(filename.Insert(filename.IndexOf(Path.GetExtension(filename), StringComparison.Ordinal), ".scale-" + scale));
            }

            return false;
        }
    }
}
