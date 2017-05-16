using System.IO;
using Windows.UI.Core;

namespace MonoCross.Utilities
{
    public class MetroDevice : Device
    {
        private readonly CoreDispatcher _dispatcher;
        public MetroDevice(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Initialize()
        {
            DirectorySeparatorChar = '\\';
            File = new Storage.MetroFile();
            ApplicationPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\Assets\\";
            DataPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\";
            Thread = new Threading.MetroThread() { Dispatcher = _dispatcher };
            Log = new Logging.BasicLogger(Path.Combine(SessionDataPath, "Log"));
            Reflector = new BasicReflector();
            Resources = new Resources.BasicResources();
            Platform = MobilePlatform.Windows;
        }
    }
}
