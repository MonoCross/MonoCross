using Microsoft.Phone.Controls;
using MonoCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MonoCross.WindowsPhone
{
    public class MXPhoneContainer : MXContainer
    {
        public MXPhoneContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        public static Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();

        protected void StartViewForController(IMXView fromView, IMXController controller, string viewPerspective)
        {
            Type viewType = Views.GetViewType(controller.ModelType, viewPerspective);
            if (viewType == null)
            {
                Console.WriteLine("View not found for " + viewPerspective);
                throw new TypeLoadException("View not found for " + viewPerspective);
            }

            var viewUri = new Uri("/" + viewType.Name + ".xaml", UriKind.Relative);

            // get the uri from the MXPhoneView attribute, if present
            object[] attributes = viewType.GetCustomAttributes(true);
            foreach (MXPhoneViewAttribute t in attributes.OfType<MXPhoneViewAttribute>())
            {
                viewUri = new Uri((t).Uri, UriKind.Relative);
                break;
            }

            // stash the model away so we can get it back when the view shows up!
            ViewModels[controller.ModelType] = controller.GetModel();

            var page = fromView as PhoneApplicationPage;
            if (page != null)
            {
                // NOTE: assumes XAML file matches type name and no sub directories
                page.NavigationService.Navigate(viewUri);
            }
            else
            {
                if (_rootFrame != null)
                {
                    _rootFrame.Navigate(viewUri);
                }

                // failure, called too early or Something Very Bad Happened(tm)...
            }
        }

        protected override void OnSetDefinitions() { }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, string viewPerspective, string navigatedUri)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => StartViewForController(fromView, controller, viewPerspective));
        }

        public override void Redirect(string url)
        {
            CancelLoad = true;
            Navigate(null, url);
        }

        static PhoneApplicationFrame _rootFrame;

        public static void Initialize(MXApplication theApp, PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;

            InitializeContainer(new MXPhoneContainer(theApp));
        }
    }
}