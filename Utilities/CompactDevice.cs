using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.ImageComposition;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using MonoCross.Navigation;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Provides a platform specific device implementation for Compact Framework targets.
    /// </summary>
    public class CompactDevice : Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDevice"/> class.
        /// </summary>
        public CompactDevice()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactDevice"/> class.
        /// </summary>
        /// <param name="dispatcherSource">The dispatcher source for returning to the UI thread.</param>
        public CompactDevice(Control dispatcherSource)
        {
            DispatcherSource = dispatcherSource;
        }

        private readonly string BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        /// <summary>
        /// Gets or sets the dispatcher source.
        /// </summary>
        /// <value>
        /// The control used to access the UI thread.
        /// </value>
        public Control DispatcherSource
        {
            get { return Thread == null ? null : Thread.DispatcherSource; }
            set
            {
                if (Thread == null) return;
                Thread.DispatcherSource = value;
            }
        }

        private const int FileUriLength = 6;
        private const string DataSubFolder = "\\Data\\";

        /// <summary>
        /// Initializes this instance with platform-specific implementations.
        /// </summary>
        public override void Initialize()
        {
            DirectorySeparatorChar = Path.DirectorySeparatorChar;
            string path = string.Concat(BasePath, "\\");
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                path = path.Substring(FileUriLength, path.Length - FileUriLength);
            }
            ApplicationPath = path;
            path = BasePath + DataSubFolder;
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                path = path.Substring(FileUriLength, path.Length - FileUriLength);
            }
            DataPath = path;

            MXContainer.RegisterSingleton<ICompositor>(typeof(GdiPlusCompositor));
            MXContainer.RegisterSingleton<IFile>(typeof(BasicFile));
            MXContainer.RegisterSingleton<ILog>(typeof(BasicLogger), () => new BasicLogger(Path.Combine(SessionDataPath, "Log")));
            MXContainer.RegisterSingleton<IThread>(typeof(CompactFrameworkThread));
            MXContainer.RegisterSingleton<IReflector>(typeof(CompactReflector));
            MXContainer.RegisterSingleton<IResources>(typeof(BasicResources));
            Platform = MobilePlatform.WindowsMobile;
        }

        public new static CompactDevice Instance
        {
            get { return Device.Instance as CompactDevice; }
            set { Device.Instance = value; }
        }

        public new static CompactFrameworkThread Thread
        {
            get { return Device.Thread as CompactFrameworkThread; }
            set { Device.Thread = value; }
        }
    }
}
