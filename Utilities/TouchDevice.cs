using System;
using MonoCross.Navigation;
using UIKit;

namespace MonoCross.Utilities
{
    public class TouchDevice : Device
    {
        public override void Initialize()
        {
            MXContainer.RegisterSingleton<Storage.IFile>(typeof(Storage.BasicFile));
            ApplicationPath = string.Empty;
            DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
            SessionDataRoot = System.IO.Path.Combine(DataPath, "session");
            MXContainer.RegisterSingleton<Logging.ILog>(typeof(Logging.BasicLogger), () => new Logging.BasicLogger(System.IO.Path.Combine(SessionDataPath, "Log")));
            MXContainer.RegisterSingleton<Threading.IThread>(typeof(Threading.TouchThread));
            MXContainer.RegisterSingleton<Resources.IResources>(typeof(Resources.WindowsResources));
            MXContainer.RegisterSingleton<IReflector>(typeof(BasicReflector));
            MXContainer.RegisterSingleton<Encryption.IEncryption>(typeof(Encryption.AesEncryption));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.TouchCompositor));
            Platform = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? MobilePlatform.iPad : MobilePlatform.iPhone;
        }
    }
}
