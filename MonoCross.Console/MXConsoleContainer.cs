using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using MonoCross.Navigation;

namespace MonoCross.Console
{
    public static class MXConsoleNavigationExtensions
    {
        public static void Back(this IMXView view)
        {
            MXConsoleContainer.Back(view);
        }
    }

    public class MXConsoleContainer : MXContainer
    {
        public MXConsoleContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        private class NavDetail
        {
            public string Path { get; set; }
            public Dictionary<string, string> Parameters { get; set; }

            public NavDetail(string path, Dictionary<string, string> parameters)
            {
                Path = path;
                Parameters = parameters;
            }
        }

        static Stack<NavDetail> NavHistory = new Stack<NavDetail>();

        public static void Initialize(MXApplication theApp)
        {
            InitializeContainer(new MXConsoleContainer(theApp));

            // non-threaded container, not needed as all input is blocking (old-school)
            Instance.ThreadedLoad = false;
        }

        public static void Back(IMXView view)
        {
            // exit if we try to go back too far
            if (!CanGoBack())
            {
                Environment.Exit(0);
            }
            else
            {
                // pop off the current view
                NavHistory.Pop();

                // prepare to re-push the current view
                NavDetail backTo = NavHistory.Pop();

                // re-display the view
                Navigate(view, backTo.Path, backTo.Parameters);
            }
        }

        public static bool CanGoBack()
        {
            if (NavHistory.Count > 1)
                return true;
            else
                return false;
        }

        protected override void OnSetDefinitions() { }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, string perspective, string navigatedUri)
        {
            // store of the stack for later
            NavHistory.Push(new NavDetail(navigatedUri, null));

            // render the view
            RenderViewFromPerspective(controller.ModelType, perspective, controller.GetModel());
        }

        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            System.Console.WriteLine("Failed to load controller: " + ex.Message);
            System.Console.WriteLine("Stack Dump");
            System.Console.WriteLine(ex.StackTrace);

            System.Diagnostics.Debug.WriteLine("Failed to load controller: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("Stack Dump");
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
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
