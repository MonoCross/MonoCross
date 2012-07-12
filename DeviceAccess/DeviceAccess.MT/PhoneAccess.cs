using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace DeviceAccess
{
  class PhoneAccess
  {
    public bool DialNumber(string PhoneNumber, bool DisplayWarning)
    {
      bool successfulDialing = false;

      // Kick off dialing using iOS's deep linking
      NSUrl url = new NSUrl("tel:" + PhoneNumber);
      if (UIApplication.SharedApplication.OpenUrl(url))
      {
        successfulDialing = true;
      }
      else if (DisplayWarning)
      {
        UIAlertView av =
            new UIAlertView("Dialing Failed", "Dialing not supported", null, "OK", null);
        av.Show();
      }

      return successfulDialing;
    }
  }
}

