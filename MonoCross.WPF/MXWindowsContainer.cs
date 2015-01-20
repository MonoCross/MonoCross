using System;
using System.Collections.Generic;

using MonoCross.Navigation;
using System.Windows.Controls;
using System.Diagnostics;

namespace MonoCross.WPF
{
    /// <summary>
    /// Represents a WPF instance of a MonoCross container.
    /// </summary>
    public class MXWindowsContainer : MXContainer
    {
        /// <summary>
        /// Cache for Models that have been loaded into views.
        /// </summary>
        public static Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();

        /// <summary>
        /// Provides an <see cref="Action{T}"/> to render the view from a loaded controller.
        /// </summary>
        public static Action<Type> NavigationHandler { get; set; }


        /// <summary>
        /// Gets or sets the frame to display rendered <see cref="MXPageView{T}"/>s.
        /// </summary>
        public static Frame NavigationFrame { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MXWindowsContainer"/> class.
        /// </summary>
        /// <param name="theApp">The application to contain.</param>
        public MXWindowsContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        /// <summary>
        /// Initializes the specified the application into a container.
        /// </summary>
        /// <param name="theApp">The application.</param>
        /// <param name="navFrame">The frame to use for page navigation.</param>
        public static void Initialize(MXApplication theApp, Frame navFrame)
        {
            InitializeContainer(new MXWindowsContainer(theApp));
            Instance.ThreadedLoad = false;
            NavigationFrame = navFrame;
        }

        /// <summary>
        /// Called when a controller is about to be loaded.
        /// </summary>
        /// <param name="controller">The controller to be loaded.</param>
        /// <param name="fromView">The view that initiated the navigation that resulted in the controller being loaded.</param>
        protected override void OnControllerLoadBegin(IMXController controller, IMXView fromView)
        {
            Debug.WriteLine("OnControllerLoadBegin");
        }

        /// <summary>
        /// Raises the controller load failed event.
        /// </summary>
        /// <param name="controller">The controller that failed to load.</param>
        /// <param name="ex">The exception that caused the load to fail.</param>
        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            Debug.WriteLine("OnControllerLoadFailed: " + ex.Message);
        }

        /// <summary>
        /// Raises the load complete event after the Controller has completed loading its Model. The View may be populated,
        /// and the derived class should check if it exists and do something with it if needed for the platform: either free it,
        /// pop off the views in a stack above it or whatever makes sense to the platform.
        /// </summary>
        /// <param name="fromView">The view that raised the navigation.</param>
        /// <param name="controller">The newly loaded controller.</param>
        /// <param name="viewPerspective">The view perspective returned by the controller load.</param>
        /// <exception cref="System.TypeLoadException">View not found for  + viewPerspective.ToString()</exception>
        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, string viewPerspective, string navigatedUri)
        {
            Debug.WriteLine("OnControllerLoadComplete");

            Type viewType = Views.GetViewType(controller.ModelType, viewPerspective);
            if (viewType != null)
            {
                // stash the model away so we can get it back when the view shows up!
                ViewModels[controller.ModelType] = controller.GetModel();

                var page = fromView as Page;
                if (NavigationHandler != null)
                {
                    // allow first crack at the view creation to the person over-riding
                    NavigationHandler(viewType);
                }
                else if (page != null)
                {
                    // start the next view
                    NavigationFrame.NavigationService.Navigate(page);
                }
                else
                {
                    RenderViewFromPerspective(controller, viewPerspective);
                    NavigationFrame.NavigationService.Navigate(Views.GetView(controller.ModelType, viewPerspective) as Page);
                }
            }
            else
            {
                Debug.WriteLine("OnControllerLoadComplete: View not found for " + viewPerspective.ToString());
                throw new TypeLoadException("View not found for " + viewPerspective.ToString());
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