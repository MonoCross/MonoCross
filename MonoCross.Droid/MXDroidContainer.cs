using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;

using MonoCross.Navigation;

namespace MonoCross.Droid
{
    public class MXDroidContainer : MXContainer
    {
        public static Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();
        public static Action<Type> NavigationHandler { get; set; }
        public static Context ApplicationContext { get; private set; }

        public MXDroidContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        public static void Initialize(MXApplication theApp, Context applicationContext)
        {
            InitializeContainer(new MXDroidContainer(theApp));
            Instance.ThreadedLoad = true;
            ApplicationContext = applicationContext;
        }

        [Obsolete]
        protected override void OnControllerLoadBegin(IMXController controller)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadBegin");
        }

        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadFailed: " + ex.Message);
        }


        /// <summary>
        /// Gets or sets the last active Context.
        /// </summary>
        /// <value>
        /// The last active <see cref="Context"/>.
        /// </value>
        /// <remarks>This is useful for figuring out your fromView if you've implemented a <see cref="NavigationHandler"/>.
        /// However, this isn't a reliable source to get the currently visible activity. </remarks>
        public Context LastContext { get; set; }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete");

            Type viewType = Views.GetViewType(viewPerspective);
            if (viewType != null)
            {
                // stash the model away so we can get it back when the view shows up!
                ViewModels[controller.ModelType] = controller.GetModel();

                LastContext = fromView as Activity ?? ApplicationContext;
                if (NavigationHandler != null)
                {
                    // allow first crack at the view creation to the person over-riding
                    NavigationHandler(viewType);
                }
                else if (LastContext != null)
                {
                    // use the last context to instantiate the new view
                    Intent intent = new Intent(LastContext, viewType);
                    intent.AddFlags(ActivityFlags.NewTask);
                    LastContext.StartActivity(intent);
                }
                else
                {
                    Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete: View not found for " + viewPerspective.ToString());
                    throw new TypeLoadException("View not found for " + viewPerspective.ToString());
                }
            }
            else
            {
                Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete: View not found for " + viewPerspective.ToString());
                throw new TypeLoadException("View not found for " + viewPerspective.ToString());
            }
        }

        public override void Redirect(string url)
        {
            Navigate(null, url);
            CancelLoad = true;
        }
    }
}