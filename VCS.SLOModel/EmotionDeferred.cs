using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum EmotionDeferred
	{
		Confident,
		Comfortable,
		Puzzled,
		Neutral,
		Worried,
		Uncomfortable,
		Disappointed,
	}

	public static class EmotionsDeferred
	{
		public static string[] GetValues()
		{
			Type t = typeof(EmotionDeferred);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}

		public static object Translate(EmotionDeferred emotionDeferred, Language language)
		{
			string translation = "";

			if (language == Language.Catalan)
			{
				switch (emotionDeferred)
				{
					case EmotionDeferred.Confident: translation = "Confiat"; break;
					case EmotionDeferred.Comfortable: translation = "Còmode"; break;
					case EmotionDeferred.Puzzled: translation = "Desconcertat"; break;
					case EmotionDeferred.Neutral: translation = "Neutral"; break;
					case EmotionDeferred.Worried: translation = "Preocupat"; break;
					case EmotionDeferred.Uncomfortable: translation = "Incòmode"; break;
					case EmotionDeferred.Disappointed: translation = "Decebut"; break;
				}
			}
			else
			{
				translation = emotionDeferred.ToString();
			}

			return translation;
		}
	}
}
