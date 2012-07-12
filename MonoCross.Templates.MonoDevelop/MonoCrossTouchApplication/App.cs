using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;

namespace ${ProjectName}
{
    public class App : MXApplication
    {
        public override void OnAppLoad()
        {
            Title = "Hello World!";

            // add controllers to navigation map
            NavigationMap.Add("", new Controllers.MessageController());

            // set navigate on load endpoint
            NavigateOnLoad = "";
        }
    }
}
