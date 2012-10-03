using System;
using System.Collections.Generic;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using MonoCross.Droid;
using MonoCross.Navigation;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Droid
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        private bool firstRun = true;
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
            MXContainer.AddView<List<Customer>>(typeof(Views.CustomerListView), ViewPerspective.Default);
            MXContainer.AddView<Customer>(typeof(Views.CustomerView), ViewPerspective.Default);
            MXContainer.AddView<Customer>(typeof(Views.CustomerEditView), ViewPerspective.Update);

            // navigate to first view
            MXContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }

        /// <summary>
        /// Custom navigation handler for Fragments
        /// </summary>
        /// <param name="viewType">The Type of the view receiving navigation</param>
        private void FragmentHandler(Type viewType)
        {
            RunOnUiThread(() =>
            {
                var transaction = SupportFragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.main, (Fragment)Activator.CreateInstance(viewType));
                if (firstRun) firstRun = false;
                else transaction.AddToBackStack(null);
                transaction.Commit();
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