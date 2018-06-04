using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.Content.Res;
using Android.Util;
using MonoCross.Utilities.Network;

namespace MonoCross.Utilities.Storage
{
    public class AndroidFile : BasicFile
    {
        [Android.Runtime.Preserve]
        public AndroidFile() { }

        #region Asset and resource helpers

        private bool _isPrepared;
        private string _packageName;
        private Android.Content.Res.Resources _resources;

        /// <summary>
        /// A <see cref="Android.Content.Res.AssetManager"/> for retrieving embedded application assets.
        /// </summary>
        public static AssetManager AssetManager => AndroidDevice.Instance.Context.Assets;

        /// <summary>
        /// Formats a fully-qualified asset path for use with an <see cref="Android.Content.Res.AssetManager"/>.
        /// </summary>
        /// <param name="filename">The asset's path.</param>
        /// <returns>A <see cref="string"/> that can be used with <see cref="AssetManager"/>.</returns>
        public string AssetFromFileName(string filename)
        {
            if (filename == null) return null;
            return filename.StartsWith(Device.ApplicationPath) ? filename.Substring(Device.ApplicationPath.Length) : filename;
        }

        /// <summary>
        /// Returns a resource identifier from a given filename.
        /// </summary>
        /// <param name="filename">The resource name.</param>
        /// <returns>The resource identifier if the resource exists, otherwise 0.</returns>
        public int ResourceFromFileName(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || filename.StartsWith("/")) return 0;
            var dot = filename.LastIndexOf(".", StringComparison.Ordinal);
            var resourceName = dot > -1 ? filename.Remove(dot) : filename;

            if (resourceName.Contains("/"))
            {
                resourceName = resourceName.Substring(resourceName.LastIndexOf("/", StringComparison.Ordinal) + 1);
            }

            //prepend an underscore if the leading char is invalid
            if (!string.IsNullOrEmpty(resourceName) && !Regex.Match(resourceName.First().ToString(CultureInfo.InvariantCulture), "[a-zA-Z_$]").Success)
            {
                resourceName = "_" + resourceName;
            }

            var isDrawable = filename.EndsWith("png") || filename.EndsWith("jpg") || filename.EndsWith("jpeg") || filename.EndsWith("gif") || filename.EndsWith("bmp");

            // replace invaild characters
            resourceName = Regex.Replace(resourceName, "[^a-zA-Z\\d_$]", "_").ToLowerInvariant();
            PrepareResources();
            return _resources.GetIdentifier(resourceName, isDrawable ? "drawable" : "raw", _packageName);
        }

        private void PrepareResources()
        {
            if (_isPrepared) return;
            _isPrepared = true;

            var context = AndroidDevice.Instance.Context;
            _packageName = context.PackageName;

            var metrics = new DisplayMetrics();
            context.WindowManager.DefaultDisplay.GetMetrics(metrics);
            _resources = new Android.Content.Res.Resources(context.Assets, metrics, context.Resources.Configuration);
        }

        public Stream GetAsset(string assetPath)
        {
            if (assetPath == null) return null;
            assetPath = AssetFromFileName(assetPath.AppendPath(string.Empty)).Trim('/');
            try { return AssetManager.Open(assetPath); }
            catch { return null; }
        }

        #endregion

        #region IFile Members

        /// <summary>
        /// Copies the specified file to the specified destination.
        /// </summary>
        /// <param name="sourcefilename">The file to copy.</param>
        /// <param name="destinationfilename">The destination of the file being copied.</param>
        public override void Copy(string sourcefilename, string destinationfilename)
        {
            if (sourcefilename == null) throw new ArgumentNullException(nameof(sourcefilename));
            if (destinationfilename == null) throw new ArgumentNullException(nameof(destinationfilename));
            var file = Read(sourcefilename);
            if (file == null) throw new FileNotFoundException("File not found.", sourcefilename);
            Save(destinationfilename, file);
        }

        public override void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwriteExisting)
        {
            if (sourceDirectoryName == null) throw new ArgumentNullException(nameof(sourceDirectoryName), "Must specify source directory");
            if (destinationDirectoryName == null) throw new ArgumentNullException(nameof(destinationDirectoryName), "Must specify destination directory");

            if (base.Exists(sourceDirectoryName))
            {
                base.CopyDirectory(sourceDirectoryName, destinationDirectoryName, overwriteExisting);
                return;
            }

            // Create the destination folder if needed
            CreateDirectory(destinationDirectoryName);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string fls in GetFileNames(sourceDirectoryName))
            {
                FileInfo flInfo = new FileInfo(fls);
                var destFile = Path.Combine(destinationDirectoryName, flInfo.Name);
                if (overwriteExisting || !base.Exists(destFile))
                {
                    Copy(sourceDirectoryName.AppendPath(flInfo.Name), destFile);
                }
            }

            // Copy all subfolders by calling CopyDirectory recursively
            foreach (string drs in GetDirectoryNames(sourceDirectoryName))
            {
                DirectoryInfo drInfo = new DirectoryInfo(drs);
                CopyDirectory(drs, Path.Combine(destinationDirectoryName, drInfo.Name), overwriteExisting);
            }
        }

        protected override byte[] ReadClear(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            byte[] file = null;
            if (base.Exists(filename))
            {
                file = base.ReadClear(filename);
            }
            else
            {
                using (var input = GetAsset(filename))
                {
                    if (input != null)
                        file = NetworkUtils.StreamToByteArray(input);
                }
            }

            if (file == null)
            {
                int resource = ResourceFromFileName(filename);
                if (resource > 0)
                {
                    var stream = _resources.OpenRawResource(resource);
                    file = NetworkUtils.StreamToByteArray(stream);
                }
            }

            if (file == null)
            {
                throw new FileNotFoundException("File not found.", filename);
            }

            return file;
        }

        public override string ReadStringClear(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            byte[] contents = ReadClear(filename);
            return Encoding.Unicode.GetString(contents);
        }

        public override long Length(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            return ReadStringClear(filename).Length;
        }

        /// <summary>
        /// Returns the names of the files that are inside of the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the files of.</param>
        public override string[] GetFileNames(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException(nameof(directoryName));
            return directoryName.StartsWith(Device.ApplicationPath) ? GetNames(directoryName, true) : base.GetFileNames(directoryName);
        }

        /// <summary>
        /// Returns the name of the directory that contains the specified file.
        /// </summary>
        /// <param name="filename">The file to get the directory of.</param>
        public override string[] GetDirectoryNames(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException(nameof(directoryName));
            return directoryName.StartsWith(Device.ApplicationPath) ? GetNames(directoryName, false) : base.GetDirectoryNames(directoryName);
        }

        private string[] GetNames(string directoryName, bool fileList)
        {
            string baseDir = directoryName.Substring(Device.ApplicationPath.Length).Trim('/');
            return AssetManager.List(baseDir).Where(d => (AssetManager.List(baseDir.AppendPath(d)).Length == 0) == fileList)
                .Select(directoryName.AppendPath).ToArray();
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified file name exists.
        /// </summary>
        /// <param name="filename">Name of the file.</param>
        /// <returns><c>true</c> if the specified file exists; otherwise, <c>false</c>.</returns>
        public override bool Exists(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return false;
            if (base.Exists(filename) || ResourceFromFileName(filename) > 0)
                return true;
            using (var asset = filename.StartsWith("/") ? null : GetAsset(filename))
                return asset != null || AssetManager.List(AssetFromFileName(filename)).Any();
        }

        #endregion
    }
}