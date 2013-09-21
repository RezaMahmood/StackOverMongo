using System;
using System.Linq;
using System.Xml.Linq;

namespace StackOverMongo.Import
{
    public static class AttributeHelper
    {
        public static string GetAttributeStringValue(this XElement element, string attributeName)
        {
            if (HasAttribute(attributeName, element))
            {
                return element.Attribute(attributeName).Value;
            }
            return String.Empty;
        }

        public static bool HasAttribute(string attributeName, XElement element)
        {
            var allAttributes = element.Attributes();
            return allAttributes.Any(x => x.Name == attributeName);
        }
    }
}
