using MonoCross.Navigation;
using MonoCross.WindowsPhone;
using System.Windows.Controls;

namespace BestSellers.WindowsPhone
{

    public class BookListView : MXPhonePage<BookList> { }

    [MXPhoneView("/Views/BookListPage.xaml")]
    public partial class BookListPage
    {
        public BookListPage()
        {
            InitializeComponent();
        }

        public override void Render()
        {
            ApplicationTitle.Text = MXContainer.Instance.App.Title;
            PageTitle.Text = Model.CategoryDisplayName;

            //listBox.DataContext = Model;
            foreach (var book in Model)
                ListBox.Items.Add(book);

            ListBox.SelectionChanged += listBox_SelectionChanged;
        }

        void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var b = e.AddedItems[0] as Book;
            if (b != null)
                MXContainer.Navigate(this, string.Format("{0}/{1}", b.CategoryEncoded, b.ISBN));
        }
    }    
}