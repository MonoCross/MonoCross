using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MonoDroid.Dialog
{
    public class DialogAdapter : BaseAdapter
    {
        private Context _context;
        private IEnumerable<Element> _elements;

        public DialogAdapter(Context context, IEnumerable<Element> elements)
        {
            _context = context;
            _elements = elements;
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
            Element element = _elements.ElementAt(position);
            return new DialogView(_context, element);
        }

        public override int Count
        {
            get { return _elements.Count(); }
        }
    }
}