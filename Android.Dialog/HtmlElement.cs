using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Uri = Android.Net.Uri;

namespace Android.Dialog
{
    public class HtmlElement : StringElement
    {
        // public string Value;

        public HtmlElement(string caption, string url)
            : base(caption, Resource.Layout.dialog_labelfieldright)
        {
            Url = Uri.Parse(url);
        }

        public HtmlElement(string caption, Uri uri)
            : base(caption, Resource.Layout.dialog_labelfieldright)
        {
            Url = uri;
        }

        public Uri Url { get; set; }

        void OpenUrl(Context context)
        {
            Intent intent = new Intent(context, typeof(HtmlActivity));
            intent.PutExtra("URL", Url.ToString());
            intent.PutExtra("Title", Caption);
            intent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            var view = base.GetView(context, convertView, parent);
            Click = (o, e) => OpenUrl(context);
            return view;
        }
    }

    [Activity]
    public class HtmlActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string url = Intent.GetStringExtra("URL");
            Title = Intent.GetStringExtra("Title");

            WebView webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;
            webview.Settings.BuiltInZoomControls = true;
            SetContentView(webview);
            webview.LoadUrl(url);
        }
    }
}