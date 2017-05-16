using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MonoCross.Droid;

namespace BestSellers.Droid.Views
{
    [Activity(Label = "Book Details")]
    public class BookView : MXListActivityView<Book>
    {
        public override void Render()
        {
            Android.Util.Log.Debug("BookView: Render", "Description: " + Model.Description);
            ListView.Adapter = new BookAdapter(this, new Dictionary<string, string>
            {
                { Model.Title.ToTitleCase(), "Number " + Model.Rank },
                { "Rank Last Week", Model.RankLastWeek },
                { "Weeks on List", Model.WeeksOnList },
                { "Price", string.Format("${0}", Model.Price) },
                { "ISBN", Model.ISBN10 },
                { "Description", Model.Description },
            }.ToList());
        }

        private class BookAdapter : ArrayAdapter<KeyValuePair<string, string>>
        {
            private readonly IList<KeyValuePair<string, string>> _source;
            private readonly Activity _context;

            public BookAdapter(Context context, IList<KeyValuePair<string, string>> objects)
                : base(context, Android.Resource.Layout.SimpleListItem1, objects)
            {
                _source = objects;
                _context = (Activity)context;
            }

            public override int GetItemViewType(int position)
            {
                return position == _source.Count - 1 ? 1 : 0;
            }

            public override int ViewTypeCount
            {
                get { return 2; }
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                //Get our object for this position
                var item = _source[position];

                // Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
                // This gives us some performance gains by not always inflating a new view
                // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
                var view = (ViewGroup)(convertView ?? _context.LayoutInflater.Inflate(GetItemViewType(position) == 0 ? Resource.Layout.ListItem : Resource.Layout.Description, parent, false));

                //Find references to each subview in the list item's view
                var tag = view.Tag as Tag ?? new Tag();
                //var imageItem = tag.ImageItem ?? (tag.ImageItem = view.FindViewById(Resource.id.imageItem) as ImageView);
                var textTop = tag.Text1 ?? (tag.Text1 = view.FindViewById<TextView>(Resource.Id.text1));
                var textBottom = tag.Text2 ?? (tag.Text2 = view.FindViewById<TextView>(Resource.Id.text2));
                view.Tag = tag;

                //Assign this item's values to the various subviews
                if (null != textTop)
                    textTop.SetText(item.Key, TextView.BufferType.Normal);
                if (null != textBottom)
                    textBottom.SetText(item.Value, TextView.BufferType.Normal);

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