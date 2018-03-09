using MonoCross.Navigation;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using System;
using System.IO;
using System.Linq;

namespace MonoCross.Utilities
{
    public class ConsoleDevice : Device
    {
        public override void Initialize()
        {
            ApplicationPath = File.DirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
            DataPath = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("dataPath") ?
                Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings.Get("dataPath")) :
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AppendPath("MXData");

            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<IThread>(new TaskThread { UiSynchronizationContext = System.Threading.SynchronizationContext.Current, });
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.GdiPlusCompositor));

            Platform = MobilePlatform.Windows;
        }

        public static new ConsoleDevice Instance
        {
            get
            {
                var device = Device.Instance as ConsoleDevice;
                if (device == null)
                {
                    device = new ConsoleDevice();
                    Device.Instance = device;
                }
                return device;
            }
            set { Device.Instance = value; }
        }
    }
}