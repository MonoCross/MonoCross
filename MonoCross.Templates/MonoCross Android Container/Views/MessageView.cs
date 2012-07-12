using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MonoCross.Navigation;
using MonoCross.Droid;

namespace $safeprojectname$.Views
{
    [Activity(Label = "Hello World!")]
    public class MessageView : MXListActivityView<string>
    {
        public override void Render()
        {
            ListView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new string[] { Model });
        }
    }
}