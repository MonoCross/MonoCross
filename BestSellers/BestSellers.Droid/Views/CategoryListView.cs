using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;

using MonoCross.Navigation;
using MonoCross.Droid;

namespace BestSellers.Droid.Views
{
    [Activity(MainLauncher = true, Label = "The New York Times Best Sellers", LaunchMode = LaunchMode.SingleTop)]
    public class CategoryListView : MXListActivityView<CategoryList>
    {
        public override void Render()
        {
            if (Model == null) return;
            var categories = Model.Where(c => c != null).Select(c => c.DisplayName).ToArray();
            ListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, categories);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            string url = string.Format(Model[position].ListNameEncoded);

            this.Navigate(url);
        }

        protected override void OnCreate(Bundle bundle)
        {
            Android.Util.Log.Debug("MainActivity", "OnCreate");

            if (MXContainer.Instance == null)
            {
                // initialize app
                MXDroidContainer.Initialize(new App(), ApplicationContext);

                // initialize views
                MXContainer.AddView<CategoryList>(typeof(CategoryListView), ViewPerspective.Read);
                MXContainer.AddView<BookList>(typeof(BookListView), ViewPerspective.Read);
                MXContainer.AddView<Book>(typeof(BookView), ViewPerspective.Read);

                // navigate to first view
                MXContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
            }

            base.OnCreate(bundle);
        }
    }
}

