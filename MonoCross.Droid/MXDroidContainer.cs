using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;

using MonoCross.Navigation;

namespace MonoCross.Droid
{
    /// <summary>
    /// Represents the MonoCross Android container.
    /// </summary>
    public class MXDroidContainer : MXContainer
    {
        /// <summary>
        /// Cache for Models that have been loaded into views.
        /// </summary>
        public static readonly Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();

        /// <summary>
        /// Provides an <see cref="Action{T}"/> to render the view from a loaded controller.
        /// </summary>
        public static Action<Type> NavigationHandler { get; set; }

        /// <summary>
        /// Return the context of the single, global Application object of the current process.
        /// </summary>
        public static Context ApplicationContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MXDroidContainer"/> class.
        /// </summary>
        /// <param name="theApp">The <see cref="MXApplication"/> to manage.</param>
        public MXDroidContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        /// <summary>
        /// Initializes a <see cref="MXDroidContainer"/>.
        /// </summary>
        /// <param name="theApp">The <see cref="MXApplication"/> to manage.</param>
        /// <param name="applicationContext">The ApplicationContext of the first activity.</param>
        public static void Initialize(MXApplication theApp, Context applicationContext)
        {
            InitializeContainer(new MXDroidContainer(theApp));
            Instance.ThreadedLoad = true;
            ApplicationContext = applicationContext;
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

        /// <summary>
        /// Occurs when a controller throws an unhandled exception from MXContainer.TryLoadController().
        /// </summary>
        /// <param name="controller">The <see cref="IMXController"/> that failed to load.</param>
        /// <param name="ex">The <see cref="Exception"/> that caused the load to fail.</param>
        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadFailed: " + ex.Message);
        }

        protected override void OnSetDefinitions() { }

        /// <summary>
        /// Occurs after a successful controller load.
        /// </summary>
        /// <param name="fromView">The <see cref="IMXView"/> that kicked off the navigation.</param>
        /// <param name="controller">The <see cref="IMXController"/> that received the navigation.</param>
        /// <param name="viewPerspective">The <see cref="ViewPerspective"/> returned by the controller load.</param>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, string viewPerspective, string navigatedUri)
        {
            Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete");

            Type viewType = Views.GetViewType(controller.ModelType, viewPerspective);
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
                    var intent = new Intent(LastContext, viewType);
                    intent.AddFlags(ActivityFlags.NewTask);
                    LastContext.StartActivity(intent);
                }
                else
                {
                    Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete: View not found for " + viewPerspective);
                    throw new TypeLoadException("View not found for " + viewPerspective);
                }
            }
            else
            {
                Android.Util.Log.Debug("MXDroidContainer", "OnControllerLoadComplete: View not found for " + viewPerspective);
                throw new TypeLoadException("View not found for " + viewPerspective);
            }
        }

        /// <summary>
        /// Cancels loading of the current controller and navigates to the specified url.
        /// </summary>
        /// <param name="url">The url of the controller to navigate to.</param>
        public override void Redirect(string url)
        {
            Navigate(null, url);
            CancelLoad = true;
        }
    }
}