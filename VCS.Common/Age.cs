using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum Age
	{
		Unknown,
		Adult,
		Child,
		Senior,
		Teen,
	}

	public static class Ages
	{
		public static string[] GetValues()
		{
			Type t = typeof(Age);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}
