using System.IO;
using System.Web;

using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.Network;
using MonoCross.Utilities.Threading;

using MonoCross.Navigation;
using MonoCross.Utilities.Resources;
using MonoCross.Web;

namespace MonoCross.Utilities
{
    public class WebDevice : Device
    {
        public WebDevice() { }

        public override void Initialize()
        {
            DirectorySeparatorChar = Path.DirectorySeparatorChar;
            MXContainer.RegisterSingleton<IThread>(typeof(TaskThread));
            MXContainer.RegisterSingleton<IEncryption>(typeof(AesEncryption));
            MXContainer.RegisterSingleton<IResources>(typeof(WindowsResources));
            MXContainer.RegisterSingleton<ImageComposition.ICompositor>(typeof(ImageComposition.GdiPlusCompositor));

            NetworkPostMethod = NetworkPostMethod.ImmediateSynchronous;

            var session = new WebSessionDictionary();
            foreach (var kvp in Session)
            {
                session.Add(kvp);
            }
            Session = session;
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
