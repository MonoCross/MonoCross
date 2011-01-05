using System.Linq;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MonoDroid.Dialog
{
    public class DialogAdapter : BaseAdapter
    {
        private readonly RootElement _rootElement;

        public DialogAdapter(RootElement rootElement)
        {
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
            return section.GetView();
        }

        public override int Count
        {
            get { return _rootElement.Sections.Count(); }
        }
    }
}