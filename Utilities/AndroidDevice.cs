using System.IO;
using Android.App;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using MonoCross.Navigation;
using MonoCross.Utilities.ImageComposition;

namespace MonoCross.Utilities
{
    public class AndroidDevice : Device
    {
        public Activity Context { get; set; }

        public AndroidDevice(Activity context)
        {
            Context = context;
        }

        public override void Initialize()
        {
            ApplicationPath = "file:///android_asset/";
            DataPath = Context.FilesDir.AbsolutePath;
            Platform = MobilePlatform.Android;

            MXContainer.RegisterSingleton<ILog>(typeof(AndroidLogger), args =>
            {
                var path = args.Length > 0 ? args[0] as string : null;
                return new AndroidLogger(path ?? Path.Combine(SessionDataPath, "Log"));
            });
            MXContainer.RegisterSingleton<IEncryption>(typeof(AndroidEncryption));
            MXContainer.RegisterSingleton<IFile>(typeof(AndroidFile));
            MXContainer.RegisterSingleton<IThread>(typeof(AndroidThread));
            MXContainer.RegisterSingleton<IReflector>(typeof(AndroidReflector));
            MXContainer.RegisterSingleton<IResources>(typeof(AndroidResources));
            MXContainer.RegisterSingleton<ICompositor>(typeof(AndroidCompositor));
        }

        public static new AndroidDevice Instance
        {
            get { return Device.Instance as AndroidDevice; }
            set { Device.Instance = value; }
        }
    }
}
