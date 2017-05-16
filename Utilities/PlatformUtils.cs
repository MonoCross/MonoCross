using System;
using System.Text.RegularExpressions;

namespace MonoCross
{
    /// <summary>
    /// Represents utility methods for querying platforms.
    /// </summary>
    public static class PlatformUtils
    {
        /// <summary>
        /// Gets a mobile platform from the HTTP request.
        /// </summary>
        /// <param name="userAgent">A <see cref="String"/> representing the HTTP request's user agent value.</param>
        /// <returns>A <see cref="MobilePlatform"/> value.</returns>
        public static MobilePlatform GetMobilePlatform(string userAgent)
        {
            var blackberry = new Regex("BlackBerry");
            var android = new Regex("Android");
            var iphone = new Regex("iPhone");
            var ipad = new Regex("iPad");
            var pre = new Regex("Pre");

            var windowsnt = new Regex("Windows NT");
            var windowsce = new Regex("Windows CE");
            var winPhone = new Regex("IEMobile");
            var windowsie = new Regex("IE");

            if (blackberry.IsMatch(userAgent))
                return MobilePlatform.BlackBerry;
            if (pre.IsMatch(userAgent))
                return MobilePlatform.WebOS;
            if (android.IsMatch(userAgent))
                return MobilePlatform.Android;
            if (iphone.IsMatch(userAgent))
                return MobilePlatform.iPhone;
            if (ipad.IsMatch(userAgent))
                return MobilePlatform.iPad;
            if (windowsnt.IsMatch(userAgent))
                return MobilePlatform.Windows;
            if (windowsce.IsMatch(userAgent))
                return MobilePlatform.WindowsMobile;
            if (winPhone.IsMatch(userAgent))
                return MobilePlatform.WinPhone;
            if (windowsie.IsMatch(userAgent))
                return MobilePlatform.WindowsIE;
            else
                return MobilePlatform.Unknown;
        }
    }
}
