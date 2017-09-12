using System;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using MonoCross.Navigation;

namespace MonoCross.Utilities
{
    public class WpfDevice : Device
    {
        private readonly Dispatcher _dispatcher;
        public WpfDevice(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Initialize()
        {
            DirectorySeparatorChar = Path.DirectorySeparatorChar;

            MXContainer.RegisterSingleton<IFile>(typeof(BasicFile));
            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<IThread>(typeof(DispatcherThread), () => new DispatcherThread { Dispatcher = _dispatcher, });
            MXContainer.RegisterSingleton<IReflector>(typeof(BasicReflector));
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.GdiPlusCompositor));

            ApplicationPath = File.DirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            DataPath = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("dataPath") ?
                Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings.Get("dataPath")) :
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AppendPath("MXData");

            MXContainer.RegisterSingleton<ILog>(typeof(BasicLogger), () => new BasicLogger(Path.Combine(SessionDataPath, "Log")));
            Platform = MobilePlatform.Windows;
        }

        public static new WpfDevice Instance
        {
            get { return Device.Instance as WpfDevice; }
        }

        public static new DispatcherThread Thread
        {
            get { return Device.Thread as DispatcherThread; }
        }
    }
}