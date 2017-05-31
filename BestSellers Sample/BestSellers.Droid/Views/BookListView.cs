using Android.App;
using Android.Views;
using Android.Widget;
using MonoCross.Droid;
using MonoCross.Navigation;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "Book List", LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    public class BookListView : MXListActivityView<BookList>
    {
        public override void Render()
        {
            Title = Model.CategoryDisplayName;
            ListView.Adapter = new CustomListAdapter(this, Model);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            string url = string.Format("{0}/{1}", Model[position].Category, Model[position].ISBN10);

            this.Navigate(url);
        }

        private class CustomListAdapter : BaseAdapter
        {
            readonly BookList _items;
            readonly Activity _context;

            public CustomListAdapter(Activity context, BookList list)
            {
                _context = context;
                _items = list;
            }

            public override int Count
            {
                get { return _items.Count; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                //Get our object for this position
                var item = _items[position];

                // Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
                // This gives us some performance gains by not always inflating a new view
                // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
                var view = (ViewGroup)(convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.ListItem, parent, false));

                //Find references to each subview in the list item's view
                var tag = view.Tag as Tag ?? new Tag();
                //var imageItem = tag.ImageItem ?? (tag.ImageItem = view.FindViewById(Resource.id.imageItem) as ImageView);
                var textTop = tag.Text1 ?? (tag.Text1 = view.FindViewById<TextView>(Resource.Id.text1));
                var textBottom = tag.Text2 ?? (tag.Text2 = view.FindViewById<TextView>(Resource.Id.text2));
                view.Tag = tag;

                //Assign this item's values to the various subviews
                if (null != textTop)
                    textTop.SetText(item.Title, TextView.BufferType.Normal);
                if (null != textBottom)
                    textBottom.SetText(item.Author, TextView.BufferType.Normal);

                //Finally return the view
                return view;
            }

            /// <summary>
            /// Tag for caching reusable views returned by <see cref="Activity.FindViewById{T}"/>
            /// </summary>
            private class Tag : Java.Lang.Object
            {
                public TextView Text1 { get; set; }
                public TextView Text2 { get; set; }
            }
        }
    }
}