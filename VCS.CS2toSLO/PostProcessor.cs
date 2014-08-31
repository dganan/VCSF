using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public static class PostContentPreProcessor
	{
		public static string PreProcessPostText(string text)
		{ 
			// Remove quotes or previous messages on reply

			text = text.RemoveHTMLTags();

			text = text.RemoveQuotes();



			// remove urls, code samples, etc

			// ...

			return text;
		}
	}
}
