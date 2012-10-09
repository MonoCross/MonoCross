using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using MonoCross.Droid;
using MonoCross.Navigation;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Droid.Views
{
    public class CustomerListView : MXListFragmentView<List<Customer>>
    {
        class CustomerAdapter : ArrayAdapter<Customer>
        {
            private List<Customer> _items = null;
            public List<Customer> Items
            {
                get { return _items; }
                set
                {
                    _items = value;
                    ((Activity)Context).RunOnUiThread(NotifyDataSetChanged);
                }
            }

            public CustomerAdapter(Context context, int textViewResourceId, List<Customer> items)
                : base(context, textViewResourceId, items)
            {
                _items = items;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View v = convertView;
                if (v == null)
                {
                    LayoutInflater li = (LayoutInflater)this.Context.GetSystemService(Context.LayoutInflaterService);
                    v = li.Inflate(Android.Resource.Layout.SimpleListItem2, null);
                }

                Customer o = Items[position];
                if (o != null)
                {
                    TextView tt = (TextView)v.FindViewById(Android.Resource.Id.Text1);
                    if (tt != null)
                        tt.Text = o.Name;
                    TextView bt = (TextView)v.FindViewById(Android.Resource.Id.Text2);
                    if (bt != null && o.Website != null)
                        bt.Text = o.Website;
                }
                return v;
            }

            public override int Count
            {
                get { return Items.Count; }
            }
        }

        private CustomerAdapter _adapter;

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Register view for model updates
            MXContainer.AddView<List<Customer>>(this);
            SetHasOptionsMenu(true);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            ((MainActivity)Activity).ReloadDetail = true;
            this.Navigate(string.Format("Customers/{0}", Model[position].ID));
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.customer_list_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add_customer:
                    ((MainActivity) Activity).ReloadDetail = true;
                    AddCustomer();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void Render()
        {
            ListAdapter = _adapter = new CustomerAdapter(Activity, 0, Model);
        }

        public override void OnViewModelChanged(object model)
        {
            _adapter.Items = (List<Customer>)model;
        }

        void AddCustomer()
        {
            this.Navigate("Customers/NEW");
        }
    }
}