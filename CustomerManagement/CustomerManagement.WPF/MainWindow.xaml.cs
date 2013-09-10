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
using MonoCross.Navigation;
using CustomerManagement.Shared.Model;
using MonoCross.WPF;

namespace CustomerManagement.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // initialize app
            MXWindowsContainer.Initialize(new CustomerManagement.App(), NavFrame);

            // initialize views
            MXWindowsContainer.AddView<List<Customer>>(typeof(Views.CustomerListView), ViewPerspective.Default);
            MXWindowsContainer.AddView<Customer>(typeof(Views.CustomerView), ViewPerspective.Default);
            MXWindowsContainer.AddView<Customer>(typeof(Views.CustomerEditView), ViewPerspective.Update);

            // navigate to first view
            MXWindowsContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
        }
    }
}