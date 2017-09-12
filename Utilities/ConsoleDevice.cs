using System;
using System.IO;
using System.Linq;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using MonoCross.Navigation;

namespace MonoCross.Utilities
{
    public class ConsoleDevice : Device
    {
        public override void Initialize()
        {
            DirectorySeparatorChar = Path.DirectorySeparatorChar;

            MXContainer.RegisterSingleton<IFile>(typeof(BasicFile));
            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<IThread>(typeof(DispatcherThread), () => new TaskThread { UiSynchronizationContext = System.Threading.SynchronizationContext.Current, });
            MXContainer.RegisterSingleton<IReflector>(typeof(BasicReflector));
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));

            ApplicationPath = File.DirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            DataPath = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("dataPath") ?
                Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings.Get("dataPath")) :
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AppendPath("MXData");

            MXContainer.RegisterSingleton<ILog>(typeof(BasicLogger), () => new BasicLogger(Path.Combine(SessionDataPath, "Log")));
            Platform = MobilePlatform.Windows;
        }

        #region singleton pattern
        private ConsoleDevice() { }
        public static new ConsoleDevice Instance
        {
            get
            {
                if (_instance == null) _instance = new ConsoleDevice();
                return _instance;
            }
        }
        private static ConsoleDevice _instance;
        #endregion
    }
}
