using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace VCS
{
	public static class LinqXmlExtensions
	{
		public static string GetAttributeValue(this XElement element, string name)
		{
			return element.Attributes().Where(x => x.Name.LocalName == name).Select(x => x.Value).FirstOrDefault();
		}

		public static IEnumerable<XElement> GetElementsByName(this XContainer element, string name)
		{
			return element.Elements().Where(x => x.Name.LocalName == name);
		}
	}
}
