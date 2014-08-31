using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS.SLOModel_OLD
{
	public enum EmotionalTestType
	{
		Mood,
		Emotional,
		Geneva_Wheel,
	}

	public static class EmotionTests
	{
		public static string[] GetValues()
		{
			Type t = typeof(EmotionalTestType);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}
	}
}
