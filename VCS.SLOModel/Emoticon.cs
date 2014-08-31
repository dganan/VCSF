using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum Emoticon
	{
		None,
		Normal,
		Happy,
		Sad,
		Angry,
		Worried,
		Surprised,
		Devilish,
		Crying,
	}

	public static class Emoticons
	{
		public static string[] GetValues()
		{
			Type t = typeof(Emoticon);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}
