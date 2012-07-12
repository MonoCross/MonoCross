using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WebHybrid.Touch
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow _window;
		UINavigationController _navController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			_window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			_navController = new UINavigationController(new WebViewController());
			
			// If you have defined a view, add it here:
			_window.AddSubview (_navController.View);
			
			// make the window visible
			_window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

