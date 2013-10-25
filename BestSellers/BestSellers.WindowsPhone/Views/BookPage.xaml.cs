using MonoCross.Navigation;
using MonoCross.WindowsPhone;
using System;
using System.Windows.Media.Imaging;

namespace BestSellers.WindowsPhone
{
    public class BookView : MXPhonePage<Book> { }

    [MXPhoneView("/Views/BookPage.xaml")]
    public partial class BookPage : BookView
    {
        public BookPage()
        {
            InitializeComponent();
        }

        public override void Render()
        {
            ApplicationTitle.Text = MXContainer.Instance.App.Title;
            PageTitle.Text = Model.Title;

            AmazonImage.Source = new BitmapImage(new Uri(Model.AmazonImageUrl, UriKind.Absolute));
            Author.Text = Model.Contributor;
            Price.Text = string.Format("${0}", Model.Price);
            Description.Text = Model.Description;
        }
    }
}