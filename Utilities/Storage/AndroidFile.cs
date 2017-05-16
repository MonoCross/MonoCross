using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Android.Content.Res;
using Android.Util;
using MonoCross.Utilities.Network;

namespace MonoCross.Utilities.Storage
{
    public class AndroidFile : BaseFile
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

        protected override byte[] ReadClear(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            if (filename == string.Empty) throw new ArgumentException(nameof(filename));

            if (!filename.StartsWith("/"))
            {
                using (var input = GetAsset(filename))
                {
                    if (input != null)
                    {
                        using (var output = new MemoryStream())
                        {
                            input.CopyTo(output);
                            return RemoveBOM(output.ToArray());
                        }
                    }
                }
                int resource = ResourceFromFileName(filename);
                if (resource > 0)
                {
                    var stream = _resources.OpenRawResource(resource);
                    return RemoveBOM(NetworkUtils.StreamToByteArray(stream));
                }
            }

            if (base.Exists(filename))
            {
                return RemoveBOM(base.ReadClear(filename));
            }
            throw new FileNotFoundException("File not found.", filename);
        }

        public override string ReadStringClear(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            return NetworkUtils.ByteArrayToStr(ReadClear(filename));
        }

        public override long Length(string filename)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            return ReadClear(filename).Length;
        }

        /// <summary>
        /// Gets the file names for a directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <returns></returns>
        public override string[] GetFileNames(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException(nameof(directoryName));
            return !directoryName.StartsWith(Device.ApplicationPath) ? Directory.GetFiles(directoryName) :
                AssetManager.List(directoryName.Substring(Device.ApplicationPath.Length).Trim('/')).Select(directoryName.AppendPath).ToArray();
        }

        /// <summary>
        /// Gets the directory names for a directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <returns></returns>
        public override string[] GetDirectoryNames(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException(nameof(directoryName));
            return Directory.GetDirectories(directoryName);
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

        /// <summary>
        /// Removes leading byte-order marks from a UTF-8 byte array, if present.
        /// </summary>
        /// <param name="bytes">The input bytes to be sanitized.</param>
        /// <returns>The bytes without a leading byte-order mark.</returns>
        private static byte[] RemoveBOM(byte[] bytes)
        {
            if (bytes.Length > 2 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return bytes.Skip(3).ToArray();

            return bytes;
        }
    }
}
