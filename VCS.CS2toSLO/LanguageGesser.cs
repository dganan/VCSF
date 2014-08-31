using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IvanAkcheurov.NTextCat.Lib.Legacy;
using System.Configuration;

namespace VCS
{
	public class LanguageGesser
	{
		public static Language IdentifyLanguage(string text)
		{
			Language lang = Language.Catalan;

			var languageIdentifier = new LanguageIdentifier(ConfigurationManager.AppSettings["LanguageModelsDirectory"]);

			IEnumerable<Tuple<string, double>> languages = languageIdentifier.ClassifyText(text, null).ToList();

			var mostCertainLanguage = languages.FirstOrDefault();

			if (mostCertainLanguage != null)
			{
				//Logger.LogMessage (text + " - " + mostCertainLanguage.Item1);

				if (mostCertainLanguage.Item1 == "catalan")
				{
					lang = Language.Catalan;
				}
				else if (mostCertainLanguage.Item1 == "spanish")
				{
					lang = Language.Spanish;
				}
				else if (mostCertainLanguage.Item1 == "italian")
				{
					lang = Language.Italian;
				}
				else if (mostCertainLanguage.Item1 == "english")
				{
					lang = Language.English;
				}
			}

			return lang;
		}
	}
}
