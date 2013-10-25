using System;
namespace MonoCross.WindowsPhone
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MXPhoneViewAttribute : Attribute
    {
        public String Uri { get; set; }

        public MXPhoneViewAttribute(String uri)
        {
            Uri = uri;
        }
    }
}