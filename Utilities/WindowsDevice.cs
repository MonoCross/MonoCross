using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoCross.Utilities;
using Windows.UI.Core;

namespace MonoCross.Utilities
{
    public class WindowsDevice : Device
    {
        private readonly CoreDispatcher _dispatcher;
        public WindowsDevice(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Initialize()
        {
            DirectorySeparatorChar = '\\';
            File = new FileSystem.WindowsFile();
            ApplicationPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\Assets\\";
            DataPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\";
            Thread = new Threading.WindowsThread() { Dispatcher = _dispatcher };
            Log = new Logging.BasicLogger(Path.Combine(SessionDataPath, "Log"));
            Reflector = new BasicReflector();
            Resources = new Resources.BasicResources();

#if WINDOWS_PHONE_APP
            Platform = MobilePlatform.WinPhone;
#elif WINDOWS_APP
            Platform = MobilePlatform.Windows;
#endif
        }
    }
}
