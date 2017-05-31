using System.IO;
using System.Windows;
using MonoCross.Navigation;

namespace MonoCross.Utilities
{
    public class WinPhoneDevice : Device
    {
        public override void Initialize()
        {
            DirectorySeparatorChar = Path.DirectorySeparatorChar;
            ApplicationPath = string.Empty;
            DataPath = "AppData\\";
            SessionDataRoot = Path.Combine(DataPath, "session");
            MXContainer.RegisterSingleton<Encryption.IEncryption>(typeof(Encryption.SLEncryption));
            MXContainer.RegisterSingleton<FileSystem.IFile>(typeof(FileSystem.SLFile));
            MXContainer.RegisterSingleton<Logging.ILog>(typeof(Logging.BasicLogger), () => new Logging.BasicLogger(Path.Combine(DataPath, "Logs\\")));
            MXContainer.RegisterSingleton<Threading.IThread>(typeof(Threading.DispatcherThread), () => new Threading.DispatcherThread { Dispatcher = Deployment.Current.Dispatcher, });
            MXContainer.RegisterSingleton<Resources.IResources>(typeof(Resources.WindowsResources));
            MXContainer.RegisterSingleton<IReflector>(typeof(BasicReflector));
            Platform = MobilePlatform.WinPhone;
        }
    }
}
