using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum Gender
	{
		Unknown = -2,
		Neutral = -1,
		Female = 0,
		Male = 1,
	}

	public static class Genders
	{
		public static string[] GetValues()
		{
			Type t = typeof(Gender);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}
