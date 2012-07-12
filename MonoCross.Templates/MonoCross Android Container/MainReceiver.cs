using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;

using MonoCross.Navigation;
using MonoCross.Droid;

namespace $safeprojectname$
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "MonoCross.MainReceiver.$guid1$" })]
    public class MainReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // initialize app
            // example: MXDroidContainer.Initialize(new MyApp.App(), context.ApplicationContext);

            // initialize views
            MXDroidContainer.AddView<string>(typeof(Views.MessageView), ViewPerspective.Default);

            // navigate to first view
            MXDroidContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }
    }
}