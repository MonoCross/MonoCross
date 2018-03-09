using MonoCross.Navigation;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Network;
using MonoCross.Utilities.Resources;
using MonoCross.Utilities.Threading;
using MonoCross.Web;
using System.Web;

namespace MonoCross.Utilities
{
    public class WebDevice : Device
    {
        public override void Initialize()
        {
            MXContainer.RegisterSingleton<IThread>(new TaskThread { UiSynchronizationContext = System.Threading.SynchronizationContext.Current, });
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));
            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.GdiPlusCompositor));

            NetworkPostMethod = NetworkPostMethod.ImmediateSynchronous;

            var session = new WebSessionDictionary();
            foreach (var kvp in Session)
            {
                session.Add(kvp);
            }
            Session = session;
        }

        public static new WebDevice Instance
        {
            get
            {
                var device = Device.Instance as WebDevice;
                if (device == null)
                {
                    device = new WebDevice();
                    Device.Instance = device;
                }
                return device;
            }
            set { Device.Instance = value; }
        }

        /// <summary>
        /// Gets the session data root path for the application.
        /// </summary>
        /// <value>The session data root path as a <see cref="String"/> instance.</value>
        public override string SessionDataAppend
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;

                if (HttpContext.Current.Session["SessionDataAppend"] == null)
                    HttpContext.Current.Session["SessionDataAppend"] = MXContainer.GetSessionId();

                return HttpContext.Current.Session["SessionDataAppend"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SessionDataAppend"] = value;
            }
        }
    }
}