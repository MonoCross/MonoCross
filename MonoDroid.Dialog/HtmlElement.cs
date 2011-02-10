using Android.Content;
using Android.Net;

namespace MonoDroid.Dialog
{
    public class HtmlElement : Element
    {
        public string Value;

        public HtmlElement(string caption, string url)
            : base(caption)
        {
            Url = Uri.Parse(url);
        }

        public HtmlElement(string caption, Uri uri)
            : base(caption)
        {
            this.Url = uri;
        }

        public Uri Url { get; set; }
    }
}