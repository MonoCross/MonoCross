using Microsoft.Phone.Tasks;

namespace DeviceAccess
{
  class PhoneAccess
  {
    public void DialNumber(string PhoneNumber)
    {
      PhoneCallTask task = new PhoneCallTask();
      task.PhoneNumber = "651-555-1212";
      task.Show();
    }
  }
}