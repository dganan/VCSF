using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum Language
	{
		Unknown = -1,
		English = 0,
		Spanish = 1,
		Catalan = 2,
		Italian = 3,
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
