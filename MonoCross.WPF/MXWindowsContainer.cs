using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using System.Windows.Controls;
using System.Diagnostics;

namespace MonoCross.WPF
{
    public class MXWindowsContainer : MXContainer
    {
        public static Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();
        public static Action<Type> NavigationHandler { get; set; }
        public static Frame NavigationFrame { get; set; }

        public MXWindowsContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        public static void Initialize(MXApplication theApp, Frame navFrame)
        {
            MXContainer.InitializeContainer(new MXWindowsContainer(theApp));
            MXContainer.Instance.ThreadedLoad = false;

            NavigationFrame = navFrame;
        }

        protected override void OnControllerLoadBegin(IMXController controller)
        {
            Debug.WriteLine("OnControllerLoadBegin");
        }

        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            Debug.WriteLine("OnControllerLoadFailed: " + ex.Message);
        }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Debug.WriteLine("OnControllerLoadComplete");

            Type viewType = Views.GetViewType(viewPerspective);
            if (viewType != null)
            {
                // stash the model away so we can get it back when the view shows up!
                ViewModels[controller.ModelType] = controller.GetModel();

                var activity = fromView as Page;
                if (NavigationHandler != null)
                {
                    // allow first crack at the view creation to the person over-riding
                    NavigationHandler(viewType);
                }
                else if (activity != null)
                {
                    // start the next view
                    NavigationFrame.NavigationService.Navigate(activity);
                }
                else
                {
                    RenderViewFromPerspective(controller, viewPerspective);
                    NavigationFrame.NavigationService.Navigate(Views.GetView(viewPerspective) as Page);
                }
            }
            else
            {
                Debug.WriteLine("OnControllerLoadComplete: View not found for " + viewPerspective.ToString());
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