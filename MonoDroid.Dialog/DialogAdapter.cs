using System.Linq;
using Android.Views;
using Android.Widget;
using Android.Content;
using Java.Lang;

namespace MonoDroid.Dialog
{
    public class DialogAdapter : BaseAdapter
    {
        private readonly RootElement _rootElement;

		public Context Context
		{
			get;
			set;
		}

        public DialogAdapter(Context context, RootElement rootElement)
        {
			this.Context = context;
            _rootElement = rootElement;
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var section = _rootElement.Sections[position];
            return section.GetView(this.Context);
        }

        public override int Count
        {
            get { return _rootElement.Sections.Count(); }
        }
    }
}