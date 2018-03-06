using System;
using System.IO;
using System.Linq;
using MonoCross.Utilities.Encryption;
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
            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<IThread>(typeof(TaskThread), () => new TaskThread { UiSynchronizationContext = System.Threading.SynchronizationContext.Current, });
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));

            ApplicationPath = File.DirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            DataPath = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("dataPath") ?
                Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings.Get("dataPath")) :
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AppendPath("MXData");

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
