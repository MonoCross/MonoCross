using System.Globalization;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Telephony;

using MonoCross.Navigation;
using MonoCross.Droid;
using Android.Dialog;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Droid.Views
{
    public class CustomerView : MXDialogFragmentView<Customer>
    {
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetHasOptionsMenu(true);
        }

        public override void Render()
        {
            Root = new RootElement("Customer Info") {
                new Section("Contact Info") {
                    new StringElement("Name", Model.Name, (int)DroidResources.ElementLayout.dialog_labelfieldbelow),
                    new StringElement("Website", Model.Website, (int)DroidResources.ElementLayout.dialog_labelfieldbelow) {
						Click = (o, e) => LaunchWeb(),
					},
                    new StringElement("Primary Phone", Model.PrimaryPhone, (int)DroidResources.ElementLayout.dialog_labelfieldbelow) {
						Click = (o, e) => LaunchDial(),
					},
                },
                new Section("General Info") {
                    new StringMultilineElement("Address", Model.PrimaryAddress.ToString()) {
						Click = (o, e) => LaunchMaps(),
					},
                    new StringElement("Previous Orders ", Model.Orders.Count.ToString(CultureInfo.InvariantCulture)),
                    new StringElement("Other Addresses ", Model.Addresses.Count.ToString(CultureInfo.InvariantCulture)),
                    new StringElement("Contacts ", Model.Contacts.Count.ToString(CultureInfo.InvariantCulture)),
                },
            };
        }

        void LaunchWeb()
        {
            var newIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(Model.Website));
            StartActivity(newIntent);
        }

        void LaunchMaps()
        {
            string googleAddress = Model.PrimaryAddress.ToString();
            googleAddress = System.Web.HttpUtility.UrlEncode(googleAddress);

            string url = string.Format("http://maps.google.com/maps?q={0}", googleAddress);
            var newIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            StartActivity(newIntent);
        }

        void LaunchDial()
        {
            string phoneNumber = PhoneNumberUtils.FormatNumber(Model.PrimaryPhone);
            var newIntent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + phoneNumber));
            StartActivity(newIntent);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.customer_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.change_customer:
                    this.Navigate(string.Format(@"Customers/{0}/EDIT", Model.ID));
                    return true;
                case Resource.Id.delete_customer:
                    this.Navigate(string.Format(@"Customers/{0}/DELETE", Model.ID));
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}