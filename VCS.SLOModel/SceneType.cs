using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum SceneType
	{
		Dialog,
		Assessment,
		Video,
		EmotionalTest,
		References,
	}

	public static class SceneTypes
	{
		public static string[] GetValues()
		{
			Type t = typeof(SceneType);

			return t.GetFields().Where(x => x.IsLiteral).Select(x=>x.Name).ToArray();
		}
	}
}