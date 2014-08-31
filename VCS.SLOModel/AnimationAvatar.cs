using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public enum AnimationAvatar
	{
		Female0,
		Female1,
		Female2,
		Female3,
		Female4,
		Female5,
		Female6,
		Female7,
		Female8,
		Female9,
		Female10,
		Female11,
		Female12,
		Female13,
		Female14,
		Female15,
		Female16,
		Female17,
		Female18,
		Female19,
		Female20,
		Female21,
		Female22,
		Female23,
		Female24,
		Female25,
		Female26,
		Female27,
		Female28,
		Female29,
		Female30,
		Female31,
		Female32,
		Female33,
		Female34,
		Female35,
		Female36,
		Female37,
		Female38,
		Female39,
		Female40,

		Male0,
		Male1,
		Male2,
		Male3,
		Male4,
		Male5,
		Male6,
		Male7,
		Male8,
		Male9,
		Male10,
		Male11,
		Male12,
		Male13,
		Male14,
		Male15,
		Male16,
		Male17,
		Male18,
		Male19,
		Male20,
		Male21,
		Male22,
		Male23,
		Male24,
		Male25,
		Male26,
		Male27,
		Male28,
		Male29,
		Male30,
		Male31,
		Male32,
		Male33,
		Male34,
		Male35,
		Male36,
		Male37,
		Male38,
		Male39,
	}
	
	public static class AnimationAvatars
	{
		public static string[] GetValues()
		{
			Type t = typeof(AnimationAvatar);

			return t.GetFields().Where(x => x.IsLiteral).Select(x => x.Name).ToArray();
		}

		public static string [] GetValues(string gender)
		{
			if (gender == "Unknown" || gender == "Neutral")
			{
				return GetValues ();
			}

			Type t = typeof(AnimationAvatar);

			return t.GetFields().Where(x => x.IsLiteral && x.Name.StartsWith (gender)).Select(x => x.Name).ToArray();
		}
	}
}
