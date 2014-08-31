using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum DialogSpecialMark
	{
		Accept,
		Cancel,
		Question, 
		Exclamation,

		medal_gold,
		medal_silver,
		medal_bronze,
		medal_first,

		star_black,
		star_blue,
		star_gold,
		star_green,
		star_orange,
		star_purple,
		star_red,
	}

	public static class DialogSpecialMarks
	{
		public static string[] GetValues()
		{
			Type t = typeof(DialogSpecialMark);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}
