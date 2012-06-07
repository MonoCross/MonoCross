using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class DialogAdapter : BaseAdapter<Section>
    {
        private readonly Context _context;

        public DialogAdapter(Context context, RootElement root)
        {
            _context = context;
            Root = root;
            Root.Context = _context;
        }

        public RootElement Root { get; set; }

        public override bool IsEnabled(int position)
        {
            var element = ElementAtIndex(position);
            return !(element is Section) && element != null;
        }

        public override int Count
        {
            get
            {
                //Get each adapter's count + 2 for the header and footer
                return Root.Sections.Sum(s => s.Elements.Count + 2);
            }
        }

        public override int ViewTypeCount
        {
            get
            {
                // ViewTypeCount is the same as Count for these,
                // there are as many ViewTypes as Views as every one is unique!
                return Count;
            }
        }

        /// <summary>
        /// Return the Element for the flattened/dereferenced position value.
        /// </summary>
        /// <param name="position">The direct index to the Element.</param>
        /// <returns>The Element object at the specified position or null if too out of bounds.</returns>
        public Element ElementAtIndex(int position)
        {
            int sectionIndex = 0;
            foreach (var s in Root.Sections)
            {
                if (position == 0)
                    return Root.Sections[sectionIndex];

                // note: plus two for the section header and footer views
                var size = s.Elements.Count + 2;
                if (position == size - 1)
                    return null;
                if (position < size)
                    return Root.Sections[sectionIndex].Elements[position - 1];
                position -= size;
                sectionIndex++;
            }

            return null;
        }

        public override Section this[int position]
        {
            get { return Root.Sections[position]; }
        }

        public override bool AreAllItemsEnabled()
        {
            return false;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var element = ElementAtIndex(position);
            if (element == null)
            {
                element = ElementAtIndex(position - 1);
                while (!(element is Section))
                    element = element.Parent;
                return ((Section)element).GetFooterView(_context, convertView, parent);
            }
            return element.GetView(_context, convertView, parent);
        }

        public void ReloadData()
        {
            if (Root != null)
            {
                NotifyDataSetChanged();
            }
        }

        /// <summary>
        /// Handles the ItemClick event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Android.Widget.AdapterView.ItemClickEventArgs"/> instance containing the event data.</param>
        public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var elem = ElementAtIndex(e.Position);
            if (elem == null) return;
            elem.Selected();
            if (elem.Click != null)
                elem.Click(sender, e);
        }

        /// <summary>
        /// Handles the ItemLongClick event of the ListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Android.Widget.AdapterView.ItemLongClickEventArgs"/> instance containing the event data.</param>
        public void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var elem = ElementAtIndex(e.Position);
            if (elem != null && elem.LongClick != null)
                elem.LongClick(sender, e);
        }
    }
}