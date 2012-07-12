using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;

namespace $safeprojectname$
{
    public class Program
    {
        static void Main(string[] args)
        {
            // initialize container
            // example: MXConsoleContainer.Initialize(new MyApp.App());

            // initialize views
            MXConsoleContainer.AddView<string>(new Views.MessageView(), ViewPerspective.Default);

            // navigate to first view
            MXConsoleContainer.Navigate(MXContainer.Instance.App.NavigateOnLoad);
        }
    }
}
