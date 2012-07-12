using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Phone.Controls;

using MonoCross.Navigation;
using System.Windows.Threading;
using System.Windows.Navigation;

namespace MonoCross.WindowsPhone
{
    public class MXPhoneContainer: MXContainer
    {
        public MXPhoneContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        public static Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();

        protected static void StartViewForController(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Type viewType = Instance.Views.GetViewType(viewPerspective);
            if (viewType == null)
            {
                Console.WriteLine("View not found for " + viewPerspective.ToString());
                throw new TypeLoadException("View not found for " + viewPerspective.ToString());
            }

            Uri viewUri = new Uri("/" + viewType.Name + ".xaml", UriKind.Relative);
            
            // get the uri from the MXPhoneView attribute, if present
            object[] attributes = viewType.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is MXPhoneViewAttribute)
                {
                    viewUri = new Uri(((MXPhoneViewAttribute)attributes[i]).Uri, UriKind.Relative);
                    break;
                }
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

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, MXViewPerspective viewPerspective)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => { StartViewForController(fromView, controller, viewPerspective); });
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

            MXContainer.InitializeContainer(new MXPhoneContainer(theApp));
        }
    }
}