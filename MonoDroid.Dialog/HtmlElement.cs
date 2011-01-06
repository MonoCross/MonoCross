using Android.Content;
using Android.Net;

namespace MonoDroid.Dialog
{
    public class HtmlElement : Element
    {
        private readonly Context _context;
        public string Value;

        public HtmlElement(Context context, string caption, string url)
            : base(caption)
        {
            _context = context;
            Url = Uri.Parse(url);
        }

        public HtmlElement(Context context, string caption, Uri uri)
            : base(caption)
        {
            _context = context;
            this.Url = uri;
        }

        public Uri Url { get; set; }
    }
}