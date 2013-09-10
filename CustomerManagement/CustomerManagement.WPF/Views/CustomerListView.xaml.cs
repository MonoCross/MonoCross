using System.Windows.Controls;
using MonoCross.WPF;

namespace CustomerManagement.WPF.Views
{
  /// <summary>
  /// Interaction logic for CustomerListView.xaml
  /// </summary>
  public partial class CustomerListView : ListViewGlue
  {
    public override void Render()
    {
      InitializeComponent();
      lbxCustomers.ItemsSource = Model;
      lbxCustomers.SelectionChanged 
        += new SelectionChangedEventHandler(lbxCustomers_SelectionChanged);
    }

    void lbxCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      MXWindowsContainer.Navigate(string.Format("Customers/{0}"
                                 , Model[lbxCustomers.SelectedIndex].ID));
    }
  }
}