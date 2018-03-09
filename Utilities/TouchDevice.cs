using MonoCross.Navigation;
using System;
using UIKit;

namespace MonoCross.Utilities
{
    public class TouchDevice : Device
    {
        public override void Initialize()
        {
            ApplicationPath = string.Empty;
            DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
            MXContainer.RegisterSingleton<Threading.IThread>(typeof(Threading.TouchThread));
            MXContainer.RegisterSingleton<Resources.IResources>(typeof(Resources.WindowsResources));
            MXContainer.RegisterSingleton<Encryption.IEncryption>(typeof(Encryption.AesEncryption));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.TouchCompositor));
            Platform = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? MobilePlatform.iPad : MobilePlatform.iPhone;
        }

        public static new TouchDevice Instance
        {
            get
            {
                var device = Device.Instance as TouchDevice;
                if (device == null)
                {
                    device = new TouchDevice();
                    Device.Instance = device;
                }
                return device;
            }
            set { Device.Instance = value; }
        }
    }
}