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
            List<Customer> items;

            public CustomerAdapter(Context context, int textViewResourceId, List<Customer> items)
                : base(context, textViewResourceId, items)
            {
                this.items = items;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View v = convertView;
                if (v == null)
                {
                    LayoutInflater li = (LayoutInflater)this.Context.GetSystemService(Context.LayoutInflaterService);
                    v = li.Inflate(Android.Resource.Layout.SimpleListItem2, null);
                }

                Customer o = items[position];
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
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetHasOptionsMenu(true);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
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
                    AddCustomer();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override View OnCreateView(LayoutInflater p0, ViewGroup p1, Android.OS.Bundle p2)
        {
            ListAdapter = new CustomerAdapter(Activity, 0, Model);
            return base.OnCreateView(p0, p1, p2);
        }

        void AddCustomer()
        {
            this.Navigate("Customers/NEW");
        }
    }
}