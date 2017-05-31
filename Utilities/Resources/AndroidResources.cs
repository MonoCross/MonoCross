using System;
using System.Globalization;
using System.Reflection;
using Android.Content.Res;
using Android.Util;
using Java.Util;

namespace MonoCross.Utilities.Resources
{
    public class AndroidResources : WindowsResources
    {
        [Android.Runtime.Preserve]
        public AndroidResources() { }

        public bool Set { get; set; }

        public Assembly Assembly { get; set; }

        public override void RemoveAllResources()
        {
            base.RemoveAllResources();
            Set = false;
        }

        private void Reset()
        {
            if (Set || Assembly == null) return;
            Device.Resources.AddResources(Assembly);
            Set = true;
        }

        public override object GetObject(string key, CultureInfo culture)
        {
            Reset();
            var resources = AndroidDevice.Instance.Context.Resources;
            var packageName = AndroidDevice.Instance.Context.PackageName;

            var id = resources.GetIdentifier(key, "drawable", packageName);
            if (id > 0) return GetResource(r => r.GetDrawable(id), culture);

            id = resources.GetIdentifier(key, "color", packageName);
            if (id > 0) return GetResource(r => r.GetColor(id), culture);

            id = resources.GetIdentifier(key, "dimen", packageName);
            if (id > 0) return GetResource(r => r.GetDrawable(id), culture);

            id = resources.GetIdentifier(key, "xml", packageName);
            if (id > 0) return GetResource(r => r.GetXml(id), culture);

            id = resources.GetIdentifier(key, "string", packageName);
            if (id > 0) return GetResource(r => r.GetString(id), culture);

            return base.GetObject(key, culture);
        }

        public override string GetString(string key)
        {
            Reset();
            var resources = AndroidDevice.Instance.Context.Resources;
            var packageName = AndroidDevice.Instance.Context.PackageName;

            var id = resources.GetIdentifier(key, "string", packageName);
            if (id > 0)
            {
                return resources.GetString(id);
            }

            id = resources.GetIdentifier(key, "xml", packageName);
            if (id > 0)
            {
                var xml = resources.GetXml(id);
                xml.MoveToContent();
                return xml.ReadOuterXml();
            }

            return base.GetString(key);
        }

        public override string GetString(string key, CultureInfo culture)
        {
            Reset();
            var id = AndroidDevice.Instance.Context.Resources.GetIdentifier(key, "string", AndroidDevice.Instance.Context.PackageName);
            if (id == 0) return base.GetString(key, culture);
            return GetResource(r => r.GetString(id), culture);
        }

        private T GetResource<T>(Func<Android.Content.Res.Resources, T> operation, CultureInfo culture = null)
        {
            Reset();
            if (culture == null || CultureInfo.CurrentUICulture.Equals(culture))
            {
                return operation(AndroidDevice.Instance.Context.Resources);
            }

            Configuration conf = AndroidDevice.Instance.Context.Resources.Configuration;
            conf.Locale = new Locale(culture.TwoLetterISOLanguageName);
            var metrics = new DisplayMetrics();
            AndroidDevice.Instance.Context.WindowManager.DefaultDisplay.GetMetrics(metrics);
            var resources = new Android.Content.Res.Resources(AndroidDevice.Instance.Context.Assets, metrics, conf);
            conf.Locale = new Locale(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            var retVal = operation(resources);
            resources = new Android.Content.Res.Resources(AndroidDevice.Instance.Context.Assets, metrics, conf);
            return retVal;
        }
    }
}
