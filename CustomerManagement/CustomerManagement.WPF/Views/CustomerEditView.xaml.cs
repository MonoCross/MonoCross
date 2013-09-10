using System.Windows.Navigation;
using CustomerManagement.Shared.Model;
using MonoCross.WPF;

namespace CustomerManagement.WPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerEditView : MXPageView<Customer>
    {
        public override void Render()
        {
            InitializeComponent();
        }
    }
}