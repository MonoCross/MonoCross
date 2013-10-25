using MonoCross.Navigation;
using MonoCross.WindowsPhone;
using System.Windows.Controls;

namespace BestSellers.WindowsPhone
{
    public class CategoryListView : MXPhonePage<CategoryList> { }

    [MXPhoneView("/Views/CategoryListPage.xaml")]
    public partial class CategoryListPage : CategoryListView
    {
        // Constructor
        public CategoryListPage()
        {
            InitializeComponent();

            ApplicationTitle.Text = MXContainer.Instance.App.Title;
            PageTitle.Text = "Categories";
        }

        public override void Render()
        {
            foreach (var category in Model)
                ListBox.Items.Add(category);

            ListBox.SelectionChanged += new SelectionChangedEventHandler(listBox_SelectionChanged);

            // remove the splash screen that was shown just before this
            NavigationService.RemoveBackEntry();
        }

        void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = e.AddedItems[0] as Category;

            MXContainer.Navigate(this, c.ListNameEncoded);
        }
    }
}