using System.Xml.Linq;

namespace MonoCross.Utilities
{
    public static class Xml
    {
        public static string GetAttributeValue(this XElement element, string attributeName)
        {
            XAttribute attribute = element.Attribute(attributeName);
            return attribute != null ? attribute.Value : string.Empty;
        }

        public static string GetElementValue(this XElement element)
        {
            return element != null ? element.Value : string.Empty;
        }

        public static string GetElementValue(this XElement element, string elementName)
        {
            XElement child = element.Element(elementName);
            return child != null ? child.Value : string.Empty;
        }
    }
}
