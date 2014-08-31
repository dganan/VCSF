using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	public static class AnimationAvatarExtension
	{
		public static AnimationAvatar SuitableAnimationAvatar(Role role, Gender gender, int index)
		{
			AnimationAvatar avatar = gender == Gender.Female ? AnimationAvatar.Female0 : AnimationAvatar.Male0;

			// string avatarName = role.ToString() + gender.ToString();

			string avatarName = gender.ToString();

			int availableAvatarsCount = AvailableAnimationAvatarsCount(avatarName);

			if (availableAvatarsCount > 0)
			{
				int avatarIndex = index % availableAvatarsCount;

				Enum.TryParse(avatarName + avatarIndex, out avatar);
			}

			return avatar;
		}

		private static int AvailableAnimationAvatarsCount(string avatarName)
		{
			int count = 0;

			AnimationAvatar tempAvatar;

			while (Enum.TryParse(avatarName + count, out tempAvatar))
			{
				count++;
			}
			
			return count;
		}
	}
}
