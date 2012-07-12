using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MonoCross.Droid;
using MonoCross.Navigation;

namespace ${Namespace}
{
    [Activity(Label = "SplashScreenActivity", Theme = "@android:style/Theme.Black.NoTitleBar", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class SplashScreenActivity: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // assign a layout with an image
            SetContentView(Resource.Layout.Splash);

            // initialize app
            MXDroidContainer.Initialize(new App(), this.ApplicationContext);

            // initialize views
			// TODO: replace the mapping from the model to the model view here
            //MXDroidContainer.AddView<ModelClassGoesHere>(typeof(Views.ModelClassGoesHereView), ViewPerspective.Default);

            // navigate to first view
            MXDroidContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }
    }
}