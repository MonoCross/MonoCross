using System;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace MonoCross.Utilities.Resources
{
    public class WindowsResources : BasicResources
    {
        public override object GetObject(string key)
        {
            return GetObject(key, CultureInfo.CurrentUICulture);
        }

        public override object GetObject(string key, CultureInfo culture)
        {
            foreach (var resource in Resources.Reverse<ResourceManager>())
            {
                try
                {
                    var retval = resource.GetObject(key, culture);
                    if (retval != null)
                        return retval;
                }
                catch (Exception e)
                {
                    Device.Log.Debug(string.Format("Object \"{0}\" not found for culture: {1}", key, culture), e);
                }
            }
            return null;
        }
    }
}
