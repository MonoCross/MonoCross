using System;
using System.Collections.Generic;
using System.IO;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using CustomerManagement.Droid.Views;
using MonoCross.Droid;
using MonoCross.Navigation;

using CustomerManagement.Shared.Model;
using Fragment = Android.Support.V4.App.Fragment;

namespace CustomerManagement.Droid
{
    [Activity(Label = "@string/ApplicationName",
        MainLauncher = true,
        Icon = "@drawable/icon",
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // assign a layout with an image
            SetContentView(Resource.Layout.Main);

            CheckFiles(ApplicationContext);

            // initialize app
            MXDroidContainer.Initialize(new App(), ApplicationContext);
            MXDroidContainer.NavigationHandler = FragmentHandler;

            // initialize views
            MXContainer.AddView<List<Customer>>(typeof(CustomerListView), ViewPerspective.Default);
            MXContainer.AddView<Customer>(typeof(CustomerView), ViewPerspective.Default);
            MXContainer.AddView<Customer>(typeof(CustomerEditView), ViewPerspective.Update);

            // navigate to first view
            MXContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }

        public bool ReloadDetail { get; set; }

        /// <summary>
        /// Custom navigation handler for Fragments
        /// </summary>
        /// <param name="viewType">The Type of the view receiving navigation</param>
        private void FragmentHandler(Type viewType)
        {
            RunOnUiThread(() =>
            {
                var perspective = MXContainer.Instance.Views.GetViewPerspectiveForViewType(viewType);
                var mxView = MXContainer.Instance.Views.GetView(perspective);
                var view = FindViewById<FrameLayout>(Resource.Id.master_fragment);

                if (mxView == null)
                {
                    if (view == null) ReloadDetail = false;
                    var targetFragment = view != null && view.ChildCount < 1 ? Resource.Id.master_fragment : Resource.Id.detail_fragment;
                    view = FindViewById<FrameLayout>(Resource.Id.detail_fragment);
                    var transaction = SupportFragmentManager.BeginTransaction();
                    if (view != null && view.ChildCount > 0 && !ReloadDetail)
                    {
                        transaction.SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentOpen);
                        transaction.AddToBackStack(null);
                    }
                    transaction.Replace(targetFragment, (Fragment)Activator.CreateInstance(viewType));
                    transaction.Commit();
                }
                else
                {
                    SupportFragmentManager.PopBackStackImmediate(null, Android.Support.V4.App.FragmentManager.PopBackStackInclusive);

                    if (view != null && !ReloadDetail)
                    {
                        var transaction = SupportFragmentManager.BeginTransaction();
                        transaction.Remove(SupportFragmentManager.FindFragmentById(Resource.Id.detail_fragment));
                        transaction.Commit();
                    }
                }
                ReloadDetail = false;
            });
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public void CheckFiles(Context context)
        {
            string documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            string dataDirectory = Path.Combine(documents, "Xml");
            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            string dataFile = Path.Combine(documents, @"Xml/Customers.xml");
            if (File.Exists(dataFile))
                return;

            Stream input = context.Assets.Open(@"Xml/Customers.xml");
            FileStream output = File.Create(dataFile);
            CopyStream(input, output);
            input.Close();
            output.Close();
        }
    }
}