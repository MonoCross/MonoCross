using MonoCross.Navigation;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using System;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace MonoCross.Utilities
{
    public class WpfDevice : Device
    {
        public WpfDevice(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        private readonly Dispatcher _dispatcher;

        public override void Initialize()
        {
            ApplicationPath = File.DirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            DataPath = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("dataPath") ?
                Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings.Get("dataPath")) :
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AppendPath("MXData");

            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<IThread>(typeof(DispatcherThread), args => new DispatcherThread { Dispatcher = args.Length > 0 ? args[0] as Dispatcher : null ?? _dispatcher, });
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.GdiPlusCompositor));

            Platform = MobilePlatform.Windows;
        }

        public static new WpfDevice Instance
        {
            get { return Device.Instance as WpfDevice; }
            set { Device.Instance = value; }
        }

        public static new DispatcherThread Thread
        {
            get { return Device.Thread as DispatcherThread; }
        }
    }
}