using Android.Content;

namespace AudioAndMicAccess
{
  class DeviceAccess
  {
    public void DialNumber(string PhoneNumber)
    {
      string uri = string.Format("tel:{0}", "612-555-1212");
      Android.Net.Uri phoneNumber = Android.Net.Uri.Parse(uri);
      StartActivity(new Intent(Intent.ActionDial, phoneNumber));
    }
  }
}
