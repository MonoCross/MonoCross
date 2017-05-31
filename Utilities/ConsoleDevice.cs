using System;
using System.IO;
using System.Linq;

namespace MonoCross.Utilities
{
    public class ConsoleDevice : Device
    {
        public override void Initialize()
        {
            DirectorySeparatorChar = Path.DirectorySeparatorChar;
            File = new Storage.BasicFile();
            ApplicationPath = File.DirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            DataPath = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("dataPath") ?
                System.Configuration.ConfigurationManager.AppSettings.Get("dataPath") :
                "%LOCALAPPDATA%\\ITRMobility";
            DataPath = Environment.ExpandEnvironmentVariables(DataPath);

            Thread = new MonoCross.Utilities.Threading.TaskThread
            {
                UiSynchronizationContext = System.Threading.SynchronizationContext.Current
            };

            Encryption = new Encryption.AesEncryption();
            Log = new Logging.BasicLogger(Path.Combine(SessionDataPath, "Log"));
            Resources = new Resources.WindowsResources();
            Reflector = new BasicReflector();
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
