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
using System.Linq;
using System.Collections.Generic;

namespace VCS
{
	public class ReferencesSceneEditor
	{
		private static List<ReferencesSceneReference> editingReferencesList;

		internal static List<ReferencesSceneReference> EditingReferencesList
		{
			get
			{
				if (editingReferencesList == null)
				{
					editingReferencesList = EditingScene.References.Select(x => x.Clone()).ToList();
				}

				return editingReferencesList;
			}
		}

		internal static string EditingSceneName { get { return EditingScene.Name; } }

		internal static string EditingSpeechText { get { return EditingScene.SpeechText; } }

		internal static Character EditingTeacherCharacter { get { return EditingScene.Character; } }

		private static ReferencesScene EditingScene
		{
			get { return SceneEditor.EditingScene as ReferencesScene; }

			set { SceneEditor.EditingScene = value; }
		}

		#region ReferencesSceneReferences manipulation methods

		internal static int RemoveReferencesSceneReference(int index)
		{
			editingReferencesList.RemoveAt(index);

			return Math.Min(index, editingReferencesList.Count - 1);
		}

		private static int MoveScenePart(int sourceIndex, int targetIndex)
		{
			if (targetIndex != sourceIndex && targetIndex >= 0 && targetIndex < editingReferencesList.Count && sourceIndex >= 0 && sourceIndex < editingReferencesList.Count)
			{
				ReferencesSceneReference scenePart = editingReferencesList[sourceIndex];

				editingReferencesList.RemoveAt(sourceIndex);

				editingReferencesList.Insert(targetIndex, scenePart);

				return targetIndex;
			}

			return sourceIndex;
		}

		internal static int MoveScenePartToLast(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, editingReferencesList.Count - 1);
		}

		internal static int MoveScenePartDown(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, sourceIndex + 1);
		}

		internal static int MoveScenePartUp(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, sourceIndex - 1);
		}

		internal static int MoveScenePartToFirst(int sourceIndex)
		{
			return MoveScenePart(sourceIndex, 0);
		}

		internal static void AddReferencesSceneReference(ReferencesSceneReference scenePart)
		{
			editingReferencesList.Add(scenePart);
		}

		internal static int DuplicateReferencesSceneReference(int index)
		{
			ReferencesSceneReference duplicated = editingReferencesList[index].Clone();

			editingReferencesList.Insert(index + 1, duplicated);

			return index + 1;
		}

		internal static string EditReference(int editingReference, string description, string url)
		{
			string error = ValidateReferenceData(description, url);

			if (error == null)
			{
				FillReferenceData(editingReferencesList[editingReference], description, url);
			}

			return error;
		}

		internal static string CreateReference(string description, string url)
		{
			ReferencesSceneReference r = new ReferencesSceneReference();

			string error = ValidateReferenceData(description, url);

			if (error == null)
			{
				FillReferenceData(r, description, url);

				editingReferencesList.Add(r);
			}

			return error;
		}

		private static string ValidateReferenceData(string description, string url)
		{
			if (String.IsNullOrWhiteSpace(description))
			{
				return "Reference description is required.";
			}

			if (String.IsNullOrWhiteSpace(url))
			{
				return "Reference url is required.";
			}

			try
			{
				new Uri(url, UriKind.Absolute);
			}
			catch 
			{
				return "Reference url in not a valid url.";
			}

			return null;
		}

		private static void FillReferenceData(ReferencesSceneReference r, string description, string url)
		{
			r.Description = description;
			r.Url = url;
		}

		#endregion

		#region scene editing methods

		internal static string AcceptEdit(string name, string speech, string characterName, string characterAvatar, string characterGender, Func<Uri, bool> navigate)
		{
			string error = ValidateSceneData(name, speech, characterName, characterAvatar, characterGender);

			if (error == null)
			{
				FillSceneData(name, speech, characterName, characterAvatar, characterGender);

				SceneEditor.GoBack(navigate);
			}

			return error;
		}

		internal static void CancelEdit(Func<Uri, bool> navigate)
		{
			editingReferencesList = null;

			SceneEditor.GoBack(navigate);
		}

		private static string ValidateSceneData(string name, string speech, string characterName, string characterAvatar, string characterGender)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				return "The name of the scene is required.";
			}

			if (String.IsNullOrWhiteSpace(speech))
			{
				return "The speech text is required.";
			}

			AnimationAvatar animationAvatar;

			if (!Enum.TryParse<AnimationAvatar>(characterAvatar, true, out animationAvatar))
			{
				return "The selected evaluator animation avatar type is not valid.";
			}

			Gender gender;

			if (!Enum.TryParse<Gender>(characterGender, true, out gender))
			{
				return "The selected evaluator gender type is not valid.";
			}

			if (editingReferencesList == null || editingReferencesList.Count() == 0)
			{
				return "The references list must contain at least one reference.";
			}

			return null;
		}

		private static void FillSceneData(string name, string speech, string characterName, string characterAvatar, string characterGender)
		{
			EditingScene.Name = name;
			EditingScene.SpeechText = speech;

			AnimationAvatar animationAvatar;

			Enum.TryParse<AnimationAvatar>(characterAvatar, true, out animationAvatar);

			Gender gender;

			Enum.TryParse<Gender>(characterGender, true, out gender);

			EditingScene.References = editingReferencesList;

			editingReferencesList = null;

			EditingScene.Character = new Character()
			{
				Name = (String.IsNullOrWhiteSpace(characterName) ? "Teacher" : characterName),
				AnimationAvatar = animationAvatar,
				UseAnimatedAvatar = true,
				Gender = gender
			};
		}

		#endregion
	}
}
