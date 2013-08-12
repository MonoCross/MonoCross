using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MonoCross.WPF;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.WPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : MXPageView<Customer>
    {
        public CustomerView()
        {
        }

        public override void Render()
        {
            InitializeComponent();
            lnkWebsite.NavigateUri = new Uri(Model.Website);
            lnkAddress.NavigateUri = new Uri(string.Format("http://maps.google.com/maps?q={0}", System.Web.HttpUtility.UrlEncode(Model.PrimaryAddress.ToString())));

            txtContacts.Content = Model.Contacts.Count;
            txtName.Content = Model.Name;
            txtOtherAddresses.Content = Model.Addresses.Count;
            txtPreviousOrders.Content = Model.Orders.Count;
            txtPrimaryPhone.Content = Model.PrimaryPhone;
        }

        private void RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            //This event handler ensures that the link opens a browser.
            //To open the website inline, don't use this event.
            string location = e.Uri.ToString();
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = location;

            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                //don't need to do this for Vista and up.
                process.StartInfo.FileName = "rundll32.exe";
                process.StartInfo.Arguments = "shell32.dll,OpenAs_RunDLL " + location;
                process.Start();
            }

            //if Handled isn't true, the browser page is loaded inline, despite anything else we do in this handler
            e.Handled = true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MXWindowsContainer.Navigate(string.Format(@"Customers/{0}/EDIT", Model.ID));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MXWindowsContainer.Navigate(string.Format(@"Customers/{0}/DELETE", Model.ID));
        }
    }
}