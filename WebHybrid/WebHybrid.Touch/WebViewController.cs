using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;


namespace WebHybrid.Touch
{
	public class WebViewController : UIViewController
	{
		UIWebView _webView;
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			SetupView();			
		}
		
		void SetupView()
		{
			_webView = new UIWebView();
			_webView.Frame = new RectangleF(0f, 0f, 320f, 422f);

			// enable the interaction between the webview content and the native application via this delegate
			_webView.Delegate = new WebViewDelegate(_webView);
	
			NSUrl url = NSUrl.FromFilename("www/container.html"); 
			// point to a web server instead of a local file...
			//NSUrl url = new NSUrl("http", "www.google.com", @"/");
			_webView.LoadRequest(new NSUrlRequest(url));
			
			this.View.AddSubview(_webView);
		}
	}
}