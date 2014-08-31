using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS.SLOModel_OLD
{
	public enum Language
	{
		Unknown = -1,
		English,
		Spanish,
		Catalan,
		Italian,
	}

	public static class Languages
	{
		public static string[] GetValues()
		{
			Type t = typeof(Language);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}
