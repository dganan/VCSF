using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VCS
{
	public class CharacterEditor
	{
		internal static Character EditingCharacter { get; set; }

		internal static bool CheckEditingCharacter(Func<Uri, bool> navigate)
		{
			if (EditingCharacter == null)
			{
				if (SceneEditor.CheckEditingScene(SceneType.Dialog, navigate))
				{
					GoBack(navigate);

					return false;
				}
			}

			return true;
		}

		private static void GoBack(Func<Uri, bool> navigate)
		{
			EditingCharacter = null;

			navigate(Pages.CharactersEditorPage);
		}

		public static bool Inserting { get; set; }

		public static string AcceptEdit(string name, bool useAnimation, byte[] image, string animationAvatarstr, string genderstr, string agestr, double? activity, double? quality, double? passivity, double? socialNetwork, Func<Uri, bool> navigate)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				return "The name of the scene part is required";
			}

			if (!useAnimation && image == null)
			{
				return "Please select a photo for avatar or use an animation";
			}

			AnimationAvatar animationAvatar;

			if (!Enum.TryParse<AnimationAvatar>(animationAvatarstr, true, out animationAvatar))
			{
				return "The selected animation avatar type is not valid";
			}

			Gender gender;

			if (!Enum.TryParse<Gender>(genderstr, true, out gender))
			{
				return "The selected gender type is not valid";
			}

			Age age;

			if (!Enum.TryParse<Age>(agestr, true, out age))
			{
				return "The selected age type is not valid";
			}
			
			EditingCharacter.Name = name;
			EditingCharacter.UseAnimatedAvatar = useAnimation;
			EditingCharacter.PhotoAvatar = image;
			EditingCharacter.AnimationAvatar = animationAvatar;
			EditingCharacter.Gender = gender;
			EditingCharacter.Age = age;

			EditingCharacter.Activity = activity;
			EditingCharacter.Quality = quality;
            EditingCharacter.Passivity = passivity;
            EditingCharacter.SocialNetwork = socialNetwork;

			if (Inserting)
			{
				StoryBoardEditor.AddCharacter(EditingCharacter);

				Inserting = false;
			}

			GoBack(navigate);

			return null;
		}

		public static void CancelEdit(Func<Uri, bool> navigate)
		{
			Inserting = false;

			GoBack(navigate);
		}

        internal static string ChangeWeights(double? activityWeigth, double? qualityWeigth, double? passivityWeigth, double? socialNetworkWeigth)
		{
			if (activityWeigth == null) return "Activity weight must be informed.";
			if (qualityWeigth == null) return "Quality weight must be informed.";
            if (passivityWeigth == null) return "Passivity weight must be informed.";
            if (socialNetworkWeigth == null) return "Social network weight must be informed.";

            if (activityWeigth.Value + qualityWeigth.Value + passivityWeigth.Value + socialNetworkWeigth.Value != 1)
			{
				return "Weights must sum 1.";
			}

			Character.ActivityWeight = activityWeigth.Value;
			Character.QualityWeight = qualityWeigth.Value;
            Character.PassivityWeight = passivityWeigth.Value;
            Character.SocialNetworkWeight = socialNetworkWeigth.Value;

			return null;
		}
	}
}
